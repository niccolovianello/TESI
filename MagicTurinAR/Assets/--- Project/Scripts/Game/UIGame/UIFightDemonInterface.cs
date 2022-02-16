
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIFightDemonInterface : MonoBehaviour
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
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.UnloadSceneAsync(currentSceneName);
        SceneManager.LoadSceneAsync(currentSceneName);
    }
}
