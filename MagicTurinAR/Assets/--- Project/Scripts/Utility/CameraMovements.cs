using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class CameraMovements : MonoBehaviour
{

#if UNITY_IOS || UNITY_ANDROID


    protected Plane Plane;
    private Transform previousCameraTransform = null;
    private Vector3 defaultCameraPosition = new Vector3(0, 0, 0);
    private float oldZoom = 0.0f;

    [Header("Objects")]
    public Camera Camera;
    public GameObject CameraFocus;

    [Header("Flag")]
    public bool flagActive = true;
    public bool activateRotation;
    public bool centerCameraOnPlayer = false;
    public bool flagCameraInDefualtPosition = false;

    [Header("Zoom Settings")]
    public int maxCameraDistance = 100;
    public int minCameraDistance = 6;
    public float yLimit = 0.5f;
    public float sensitivityAngle = 0.5f;
    public float edgeMaxZoom = 1;
    public Vector3 offsetDefaultCameraPosition = new Vector3(7, 7, 7);

    public CameraMovements(Camera camera, GameObject cameraFocus)
    {
        Camera = camera;
        CameraFocus = cameraFocus;

    }

    private void Start()
    {
        Debug.Log("CameraMovementsStart");
        if (Camera == null)
            foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
            {
                if (nt.isLocalPlayer)
                {
                    Camera = nt.playerCamera;
                    CameraFocus = nt.gameObject;
                
                }
            }
        previousCameraTransform = Camera.gameObject.transform;
        defaultCameraPosition = CameraFocus.transform.position + offsetDefaultCameraPosition;
        StartCoroutine(SetCameraInDefaultPosition());


    }
    private void Update()
    {
        
        if (!flagActive)
            return;

        if (flagCameraInDefualtPosition)
        {

            StartCoroutine(SetCameraInDefaultPosition());

        }

        if (centerCameraOnPlayer)
        {
            Camera.transform.LookAt(CameraFocus.transform);

        }

        if (Camera.transform.position.y < yLimit)
        {
            Debug.Log("Troppo basso");
            Camera.transform.position = previousCameraTransform.position;
            return;
        }

        //if (Vector3.Distance(CameraFocus.transform.position, Camera.transform.position) > maxCameraDistance)
        //{
        //    Camera.transform.position = previousCameraTransform.position;
        //    return;
        //}


        //Update Plane
        if (Input.touchCount >= 1)
            Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount == 1 && !centerCameraOnPlayer)
        {

            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Camera.transform.Translate(Delta1, Space.World);

            }

        }



        //Pinch
        if (Input.touchCount >= 2)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            if (activateRotation && pos2b != pos2 && (Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal) > sensitivityAngle || Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal) < -sensitivityAngle))
                Camera.transform.RotateAround(pos1, Plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, Plane.normal));


            //calc zoom
            var zoom = (Vector3.Distance(pos1, pos2) /
                           Vector3.Distance(pos1b, pos2b));

            //edge case
            if (zoom == 0 || zoom > 4)
            {
                return;
            }


            //maxZoom Limit
            if (Vector3.Distance(Camera.gameObject.transform.position, CameraFocus.transform.position) > maxCameraDistance)
            {
                Debug.Log("Maggiore");
                if (zoom < oldZoom)
                {
                    return;
                }

            }


            //minimumZoom Limit
            if (Vector3.Distance(Camera.gameObject.transform.position, CameraFocus.transform.position) < minCameraDistance)
            {
                Debug.Log("Minore");
                if (zoom > oldZoom)
                {
                    return;
                }

            }


            // z limit





            //Move cam amount the mid ray
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);
            previousCameraTransform.position = Camera.gameObject.transform.position;
            oldZoom = zoom;



        }

    }

    public IEnumerator SetCameraInDefaultPosition()
    {
        
        centerCameraOnPlayer = true;
        flagCameraInDefualtPosition = false;
        if (flagCameraInDefualtPosition)
            Debug.Log("not set correctly");
        else
            Debug.Log("Set correctly");
        defaultCameraPosition = CameraFocus.transform.position + offsetDefaultCameraPosition;
        float elapsedTime = 0;
        float waitTime = 2f;

        Debug.Log(defaultCameraPosition);
        while (elapsedTime < waitTime)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, defaultCameraPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }



    }


    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    protected Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (Plane.Raycast(rayNow, out var enterNow))
            return rayNow.GetPoint(enterNow);

        return Vector3.zero;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + transform.up);
    }

    #region UIFunctions
    public void ButtonCenterCameraOnPlayer()
    {
        if (!centerCameraOnPlayer)
        {
            flagCameraInDefualtPosition = true;
            centerCameraOnPlayer = true;
        }
            
        else
            centerCameraOnPlayer = false;
    }

    public void FreeOrAutomaticRotation()
    {
        if (activateRotation)
        {
            activateRotation = false;
        }
        else {
            activateRotation = true;
        }
    
    }

    #endregion

#endif


}

