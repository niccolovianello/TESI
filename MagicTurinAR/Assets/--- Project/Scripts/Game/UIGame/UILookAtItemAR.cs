using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class UILookAtItemAR : MonoBehaviour
{

    public CanvasRenderer startingPanel;
    public void OnClickBackButton()
    {
        GameObject arSceneManager = GameObject.Find("ARSceneManager");
        Destroy(arSceneManager.gameObject);
        GameObject gameManager = GameObject.Find("Loader");
        GameManager gm = gameManager.GetComponent<GameManager>();

        SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("AR_LookAtItem"));
        gm.networkPlayerCamera.enabled = true;
        gm.audioListener.enabled = true;
        gm.EnableMainGame();
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.RenderPlayerBody();
        }
    }

    public void CloseStartingPanel()
    {
        startingPanel.gameObject.SetActive(false);
    }
    
}
