
using System.Collections;
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
        MissionsManager mm = FindObjectOfType<MissionsManager>();
        
        if (mm.currentMission.playerType == MissionSO.PlayerType.Hunter)
        {
            string currentSceneName = mm.currentMission.sceneName;
            SceneManager.UnloadSceneAsync(currentSceneName);
            SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
        }
        
        else
        {
            SceneManager.UnloadSceneAsync("AR_EnemyFight");
            SceneManager.LoadSceneAsync("AR_EnemyFight", LoadSceneMode.Additive);
        }

    }
}
