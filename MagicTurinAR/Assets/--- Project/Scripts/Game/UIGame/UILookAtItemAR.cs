using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using Niantic.ARDKExamples.Helpers;
using UnityEngine.UI;

public class UILookAtItemAR : MonoBehaviour
{

    public CanvasRenderer startingPanel;
    public Sprite placeObjectSprite, interactObjectSprite;
    public Button buttonSwitchModeInteraction;
    private CustomARHitTest customARHitTest;

    private ARCursorRenderer _cursorRenderer;

    private void Start()
    {
        _cursorRenderer = FindObjectOfType<ARCursorRenderer>();
        customARHitTest = FindObjectOfType<CustomARHitTest>();
    }
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

    public void SwitchInteraction()
    {
        customARHitTest.flagInteraction = !customARHitTest.flagInteraction;
        if (customARHitTest.flagInteraction)
        {
            buttonSwitchModeInteraction.GetComponent<Image>().sprite = placeObjectSprite;
            _cursorRenderer.enabled = false;
        }

        else
        {
            buttonSwitchModeInteraction.GetComponent<Image>().sprite = interactObjectSprite;
            _cursorRenderer.enabled = true;
        }

    }

 

}
