using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using Niantic.ARDK.Extensions.Meshing;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using UnityEngine.SceneManagement;
using Niantic.ARDKExamples.Helpers;
using UnityEngine.UI;

public class UIDestroyGem : MonoBehaviour
{
    public Canvas windowToBackToTheGame;
    public Image  restartWindow;
    public CanvasRenderer tipPanel;

    private int _nfragment;

    private void Start()
    {
        // FindObjectOfType<Camera>().gameObject.GetComponent<PlaceDynamicARObject>().enabled = false;
    }

    public void BackToMainGame(bool gemDestroyed)
    {

        GameManager gameManager = FindObjectOfType<GameManager>();
        NetworkPlayer networkPlayer = gameManager.networkPlayer;

        if (gemDestroyed)
        {
           
            networkPlayer.CmdSendWhiteMagicFromGem(_nfragment);
            
        }
        
        gameManager.EnableMainGame();
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.RenderPlayerBody();
        }
        gameManager.PlayerCameraObject.SetActive(true);
        SceneManager.UnloadSceneAsync("AR_DestroyGem");

    }

    public void ExitFromSceneNoWhiteFragment()
    {
        BackToMainGame(false);
    }

    public void ExitFromSceneYesWhiteFragment()
    {
        BackToMainGame(true);
    }

    public void OpenBackToGameWindow(int nfrag)
    {
        windowToBackToTheGame.enabled = true;
        _nfragment = nfrag;
    }

    public void CloseBackToGameWindow()
    {
        windowToBackToTheGame.enabled = false;
    }
    
    public void OpenRestartWindow()
    {
        restartWindow.gameObject.SetActive(true);
        Debug.Log("open");
    }

    public void CloseRestartWindow()
    {
        FindObjectOfType<PlaceDynamicARObject>().SetCounterTouchToZero();
        restartWindow.gameObject.SetActive(false);
        
        // FindObjectOfType<ARMeshManager>().UseInvisibleMaterial = false;
        tipPanel.gameObject.SetActive(true);
    }

    public void CloseStartingPanel()
    {
        tipPanel.gameObject.SetActive(false);
        FindObjectOfType<PlaceDynamicARObject>().LetTouch = true;
        // FindObjectOfType<Camera>().gameObject.GetComponent<PlaceDynamicARObject>().enabled = true;

        // FindObjectOfType<ARMeshManager>().UseInvisibleMaterial = true;
    }
}
