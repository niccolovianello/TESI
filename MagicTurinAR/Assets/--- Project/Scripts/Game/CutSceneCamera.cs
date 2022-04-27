using System.Collections;
using UnityEngine;

public class CutSceneCamera : MonoBehaviour
{
    [Range(20, 150)]
    [SerializeField] private int startFov = 120;

    private Camera _thisCamera;

    private void Start()
    {
        _thisCamera = GetComponent<Camera>();
        _thisCamera.fieldOfView = startFov;

        transform.parent = FindObjectOfType<Explorer>().transform;
    }

    public void MoveCam(Vector3 startPos, Camera cam)
    {
        StartCoroutine(Dolly(startPos, cam));
    }

    private IEnumerator Dolly(Vector3 startPos, Camera cam)
    {
        var elapsedTime = 0f;

        var endPos = cam.transform.position;
        
        while (elapsedTime < 1)
        {
            var position = Vector3.Lerp(startPos, endPos, elapsedTime);
            
            var trans = transform;
            trans.position = position;
            
            var rotation = Quaternion.Lerp(trans.rotation, cam.transform.rotation, elapsedTime);
            transform.rotation = rotation;

            elapsedTime += Time.deltaTime;

            yield return new WaitForSeconds(.01f);
        }

        elapsedTime = 0f;

        while (elapsedTime < 1)
        {
            var fov = Mathf.Lerp(_thisCamera.fieldOfView, cam.fieldOfView, elapsedTime);
            _thisCamera.fieldOfView = fov;

            elapsedTime += Time.deltaTime;
            
            yield return new WaitForSeconds(.01f);
        }
        
        FindObjectOfType<CutSceneManager>().DisableCutSceneCamera();
        cam.gameObject.SetActive(true);

        _thisCamera.fieldOfView = startFov;
    }
}