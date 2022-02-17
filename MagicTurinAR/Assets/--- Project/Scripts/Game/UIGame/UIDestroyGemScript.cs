using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using UnityEngine.SceneManagement;

public class UIDestroyGemScript : MonoBehaviour
{
    public Canvas windowToBackToTheGame;

    public void BackToMainGame()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        NetworkPlayer networkPlayer = gameManager.networkPlayer;
        gameManager.EnableMainGame();
        networkPlayer.RenderPlayerBody();
        gameManager.PlayerCameraObject.SetActive(true);
        SceneManager.UnloadSceneAsync("AR_DestroyGem");
        
    }

    public void OpenBackToGameWindow()
    {
        windowToBackToTheGame.enabled = true;
    }

    public void CloseBackToGameWindow()
    {
        windowToBackToTheGame.enabled = false;
    }
}
