
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
        string currentSceneName = FindObjectOfType<MissionsManager>().currentMission.sceneName;
        StartCoroutine(ReloadScene(currentSceneName));
    }

    private IEnumerator ReloadScene(string currentSceneName)
    {
        var asyncLoadLevel = SceneManager.UnloadSceneAsync(currentSceneName);
        
        while (!asyncLoadLevel.isDone){
            Debug.Log("Loading the Scene"); 
            yield return null;
        }

        SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive);
    }
}
