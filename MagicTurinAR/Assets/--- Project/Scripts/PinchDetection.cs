using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinchDetection : MonoBehaviour
{
    [SerializeField] private float cameraSpeedMovement = 50f;
    [SerializeField] private float cameraMinDistanace = 20f;
    [SerializeField] private float cameraMaxDistance = 300f;
    [SerializeField] private Transform target;
    private TouchControls controls;
    private Coroutine zoomCoroutine;

    public Transform cameraTransform;

    private void Awake()
    {
        controls = new TouchControls();

    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();       
    }
    private void Start()
    {
        controls.Touch.SecondaryTouchContact.started += _ => ZoomStart();
        controls.Touch.SecondaryTouchContact.canceled += _ => ZoomEnd();
    }
    private void Update()
    {
        transform.LookAt(target);
    }

    private void ZoomStart()
    {
        zoomCoroutine = StartCoroutine(ZoomDetection());
    }

    private void ZoomEnd()
    {
        StopCoroutine(zoomCoroutine);
    }

    IEnumerator ZoomDetection() {
        float previousDistance = 0f, distance = 0f;

        while (true)
        {
            distance = Vector2.Distance(controls.Touch.PrimaryFingerPosition.ReadValue<Vector2>(), controls.Touch.SecondaryFingerPosition.ReadValue<Vector2>());

            //Detection
            if (distance > previousDistance)
            {
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.y -= 1;
                if(targetPosition.y > cameraMinDistanace)
                    cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSpeedMovement);


            }

            else if (distance < previousDistance)
            {
                Vector3 targetPosition = cameraTransform.position;
                targetPosition.y += 1;
                if (targetPosition.y < cameraMaxDistance)
                    cameraTransform.position = Vector3.Slerp(cameraTransform.position, targetPosition, Time.deltaTime * cameraSpeedMovement);
            }
            previousDistance = distance;
            yield return null;
        }
    }
}
