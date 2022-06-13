using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MemoryUIManager : MonoBehaviour
{
    [SerializeField] private CanvasRenderer helpUiPanel;
    private MissionsManager mm;
    public void CloseHelpUiPanel()
    {
        helpUiPanel.gameObject.SetActive(false);
    }

    public void RestartLevel()
    {
        mm = FindObjectOfType<MissionsManager>();
        SceneManager.UnloadSceneAsync(mm.currentMission.sceneName);
        SceneManager.LoadSceneAsync(mm.currentMission.sceneName, LoadSceneMode.Additive);
    }
}
