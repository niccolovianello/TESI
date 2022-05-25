using System.Collections;
using UnityEngine;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class CameraMovements : MonoBehaviour
{

#if UNITY_IOS || UNITY_ANDROID


    private Plane _plane;
    private Vector3 _defaultCameraPosition = new Vector3(0, 0, 0);
    private float _oldZoom = 0.0f;

    [Header("Objects")]
    public Camera Camera;
    public GameObject cameraFocus;

    [Header("Flag")]
    public bool flagActive = true;
    private bool flagActiveTouch = true;
    public bool activateRotation;
    public bool centerCameraOnPlayer = false;
    public bool flagCameraInDefaultPosition = false;

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
        this.cameraFocus = cameraFocus;
    }

    public Vector3 GetDefaultPosition => _defaultCameraPosition;

    private void Start()
    {
        flagActiveTouch = true;
        Debug.Log("CameraMovementsStart");
        if (Camera == null)
            foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
            {
                if (nt.isLocalPlayer)
                {
                    Camera = nt.playerCamera;
                    cameraFocus = nt.gameObject;
                
                }
            }

        _defaultCameraPosition = cameraFocus.transform.position + offsetDefaultCameraPosition;
        ResetCameraPosition();
    }
    
    
    private void Update()
    {
        
        if (!flagActive)
            return;

        if (flagCameraInDefaultPosition)
        {

            StartCoroutine(SetCameraInDefaultPosition());

        }

        if (centerCameraOnPlayer)
        {
            Camera.transform.LookAt(cameraFocus.transform);

        }

        //Update Plane
        if (Input.touchCount >= 1 && flagActiveTouch)
            _plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        //Scroll
        if (Input.touchCount == 1 && !centerCameraOnPlayer && flagActiveTouch)
        {

            Delta1 = PlanePositionDelta(Input.GetTouch(0));
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Camera.transform.Translate(Delta1, Space.World);

            }

        }



        //Pinch
        if (Input.touchCount >= 2 && flagActiveTouch)
        {
            var pos1 = PlanePosition(Input.GetTouch(0).position);
            var pos2 = PlanePosition(Input.GetTouch(1).position);
            var pos1b = PlanePosition(Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition);
            var pos2b = PlanePosition(Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition);

            if (activateRotation && pos2b != pos2 && (Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, _plane.normal) > sensitivityAngle || Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, _plane.normal) < -sensitivityAngle))
                Camera.transform.RotateAround(pos1, _plane.normal, Vector3.SignedAngle(pos2 - pos1, pos2b - pos1b, _plane.normal));


            //calc zoom
            var zoom = (Vector3.Distance(pos1, pos2) /
                           Vector3.Distance(pos1b, pos2b));

            //edge case
            if (zoom == 0 || zoom > 4)
            {
                return;
            }


            //maxZoom Limit
            if (Vector3.Distance(Camera.gameObject.transform.position, cameraFocus.transform.position) > maxCameraDistance)
            {
                Debug.Log("Maggiore");
                ResetCameraPosition();
                return;

            }


            //minimumZoom Limit
            if (Vector3.Distance(Camera.gameObject.transform.position, cameraFocus.transform.position) < minCameraDistance)
            {
                Debug.Log("Minore");

                ResetCameraPosition();
                return;

            }



        
            Camera.transform.position = Vector3.LerpUnclamped(pos1, Camera.transform.position, 1 / zoom);
            _oldZoom = zoom;

        }

    }

    public void ResetCameraPosition()
    {
        StartCoroutine(SetCameraInDefaultPosition());
    }

    private IEnumerator SetCameraInDefaultPosition()
    {

        flagActiveTouch = false;
        centerCameraOnPlayer = true;
        flagCameraInDefaultPosition = false;
        if (flagCameraInDefaultPosition)
            Debug.Log("not set correctly");
        else
            Debug.Log("Set correctly");
        _defaultCameraPosition = cameraFocus.transform.position + offsetDefaultCameraPosition;
        float elapsedTime = 0;
        float waitTime = 2f;

        Debug.Log(_defaultCameraPosition);
        while (elapsedTime < waitTime)
        {
            Camera.transform.position = Vector3.Lerp(Camera.transform.position, _defaultCameraPosition, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;

            // Yield here
            yield return null;
        }
        flagActiveTouch = true;

    }


    private Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase != TouchPhase.Moved)
            return Vector3.zero;

        //delta
        var rayBefore = Camera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = Camera.ScreenPointToRay(touch.position);
        if (_plane.Raycast(rayBefore, out var enterBefore) && _plane.Raycast(rayNow, out var enterNow))
            return rayBefore.GetPoint(enterBefore) - rayNow.GetPoint(enterNow);

        //not on plane
        return Vector3.zero;
    }

    private Vector3 PlanePosition(Vector2 screenPos)
    {
        //position
        var rayNow = Camera.ScreenPointToRay(screenPos);
        if (_plane.Raycast(rayNow, out var enterNow))
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
            flagCameraInDefaultPosition = true;
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

#if UNITY_STANDALONE_WIN
    public Camera Camera;
    public GameObject cameraFocus;

    public void ButtonCenterCameraOnPlayer()
    {
        Debug.Log("On PC this button doesn't work!");
    }

    public void FreeOrAutomaticRotation()
    {
        Debug.Log("On PC this button doesn't work!");

    }

#endif

}

