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

    public void MoveCamera(Vector3 startPos, Vector3 endPos)
    {
        cutSceneCamera.MoveCam(startPos, endPos);
    }

}
