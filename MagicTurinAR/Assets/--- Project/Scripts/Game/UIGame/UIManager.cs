using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menu_explorer;
    [SerializeField] private GameObject menu_wiseman;
    [SerializeField] private GameObject menu_hunter;

    private GameObject menu;

    [SerializeField] private GameObject GUI_explorer;
    [SerializeField] private GameObject GUI_wiseman;
    [SerializeField] private GameObject GUI_hunter;
    

    [SerializeField] private MagicPlayer player;
    private NetworkPlayer networkplayer;

    public FirebaseManager firebaseManager;
    public StoreData storeData;

    [Header("WindowDestroyGemWiseman")]
    public Canvas windowToDestroyGemWiseman;
   

    [Header("WindowDestroyGemHunter")]
    public Canvas windowToDestroyGemHunter;
    public string sceneToDestroyGemName;
    
    
    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        storeData = FindObjectOfType<StoreData>();
        player = FindObjectOfType<MagicPlayer>();

        foreach (NetworkPlayer pl in FindObjectsOfType<NetworkPlayer>())
        {
            if (pl.isLocalPlayer)
            {
                networkplayer = pl;
            }
        }

        switch (networkplayer.TypePlayerEnum)
        {
            case NetworkPlayer.TypePlayer.Explorer:

                GUI_hunter.gameObject.SetActive(false);
                GUI_wiseman.gameObject.SetActive(false);
                GUI_explorer.gameObject.SetActive(true);
                
                menu_explorer.SetActive(false);
                menu = menu_explorer;
                
                break;

            case NetworkPlayer.TypePlayer.Wiseman:

                GUI_explorer.gameObject.SetActive(false);
                GUI_hunter.gameObject.SetActive(false);
                GUI_wiseman.gameObject.SetActive(true);
                
                menu_wiseman.SetActive(false);
                menu = menu_wiseman;

                break;

            case NetworkPlayer.TypePlayer.Hunter:

                GUI_explorer.gameObject.SetActive(false);
                GUI_wiseman.gameObject.SetActive(false);
                GUI_hunter.gameObject.SetActive(true);

                menu_hunter.SetActive(false);
                menu = menu_hunter;

                break;

            default:

                break;

        }

        player.InitializeInventory();



        //player.SetUIManager(this);
    }

    
    

    //private void Update()
    //{
    //    if (Input.GetKey(KeyCode.O))
    //    {
    //        onClickSaveButton();
    //    }
    //}
 

    public void ToggleBackPack()
    {
        menu.SetActive(!menu.activeSelf);
        Debug.Log("toggle");
    }
    
    

    public void onClickSaveButton()
    {
        Debug.Log("Button Save clicked");
        firebaseManager.SaveData(storeData);
    }

    public void OpenWindowToDestroyGemWiseman()
    {
        windowToDestroyGemWiseman.enabled = true;
    }

    public void CloseWindowToDestroyGemWiseman()
    {
        windowToDestroyGemWiseman.enabled = false;
    }

    public void SendSceneDestroyGem()
    {
        foreach (MagicItemSO item in ItemAssets.Instance.magicInventorySO.items)
        {
            if (item.id == 1000 && item.prefab.GetComponent<MagicItem>().amount > 0) // Gem specific code
            {
                item.prefab.GetComponent<MagicItem>().amount--;
                FindObjectOfType<UIInventory>().UpdateGemsCount(item.prefab.GetComponent<MagicItem>().amount);
                networkplayer.CmdDestroyGem();

            }


        }




        
        CloseWindowToDestroyGemWiseman();
       

    }

    public void OpenWindowToDestroyGemHunter()
    {
        windowToDestroyGemHunter.enabled = true;

    }

    public void CloseWindowToDestroyGemHunter()
    {
        windowToDestroyGemHunter.enabled = false;

    }

    public void OpenSceneDestroyGem()
    {
        CloseWindowToDestroyGemHunter();
        GameManager gm = FindObjectOfType<GameManager>();
        SceneManager.LoadSceneAsync(sceneToDestroyGemName, LoadSceneMode.Additive);
        gm.DisableMainGame();
        networkplayer.NotRenderPlayerBody();
        gm.PlayerCameraObject.SetActive(false);
    }
}