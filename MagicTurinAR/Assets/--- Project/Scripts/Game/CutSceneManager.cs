using UnityEngine;

public class CutSceneManager : MonoBehaviour
{

    [SerializeField] private CutSceneCamera cutSceneCamera;

    public void EnableCutSceneCamera()
    {
        cutSceneCamera.gameObject.SetActive(true);
    }

    public void DisableCutSceneCamera()
    {
        cutSceneCamera.gameObject.SetActive(false);
    }

    public void MoveCamera(Vector3 startPos, Camera cam)
    {
        cutSceneCamera.MoveCam(startPos, cam);
    }

}
