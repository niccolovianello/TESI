
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_EnemyFight : MonoBehaviour
{
    public Canvas canvasWindowPlayerDefeat;
    public Button buttonPlayerDefeatOK;

    public void PlayerDefeatedByDemon()
    {
        canvasWindowPlayerDefeat.enabled = true;
    }

    public void RestartFight()
    {
        canvasWindowPlayerDefeat.enabled = false;
        string currentSceneName = FindObjectOfType<MissionsManager>().currentMission.sceneName;
        SceneManager.UnloadSceneAsync(currentSceneName);
        SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
    }
}
