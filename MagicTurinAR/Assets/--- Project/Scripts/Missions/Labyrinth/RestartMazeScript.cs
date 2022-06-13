using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartMazeScript : MonoBehaviour
{
    private MissionsManager mm;
    public void RestartLevel()
    {
        mm = FindObjectOfType<MissionsManager>();
        SceneManager.UnloadSceneAsync(mm.currentMission.sceneName);
        SceneManager.LoadSceneAsync(mm.currentMission.sceneName, LoadSceneMode.Additive);
    }
}
