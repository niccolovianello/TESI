using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using UnityEngine.SceneManagement;

public class UIDestroyGem : MonoBehaviour
{
    public Canvas windowToBackToTheGame;
    public int nfragment;

    public void BackToMainGame()
    {

        
        GameManager gameManager = FindObjectOfType<GameManager>();
        NetworkPlayer networkPlayer = gameManager.networkPlayer;
        gameManager.EnableMainGame();
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.RenderPlayerBody();
        }
        networkPlayer.CmdSendWhiteMagicFromGem(nfragment);
        gameManager.PlayerCameraObject.SetActive(true);
        SceneManager.UnloadSceneAsync("AR_DestroyGem");
        
    }

    public void OpenBackToGameWindow(int nfrag)
    {
        windowToBackToTheGame.enabled = true;
        this.nfragment = nfrag;
    }

    public void CloseBackToGameWindow()
    {
        windowToBackToTheGame.enabled = false;
    }
}
