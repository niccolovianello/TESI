using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using MirrorBasics;
using TMPro;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject menu_explorer;
    [SerializeField] private GameObject menu_wiseman;
    [SerializeField] private GameObject menu_hunter;

    private GameObject menu;

    [SerializeField] private GameObject GUI_explorer;
    [SerializeField] private GameObject GUI_wiseman;
    [SerializeField] private GameObject GUI_hunter;

    private GameObject distanceWarningScreenSpace;

    public Image gpsAccuracy;


    [SerializeField] private MagicPlayer player;
    private NetworkPlayer networkplayer;

    public FirebaseManager firebaseManager;
    public StoreData storeData;

    [Header("WindowDestroyGemWiseman")]
    public Canvas windowToDestroyGemWiseman;
   

    [Header("WindowDestroyGemHunter")]
    public Canvas windowToDestroyGemHunter;
    public string sceneToDestroyGemName;


    [Header("DistanceBetweenPlayersChecker")]
    [SerializeField] private GameObject imageIndicatorGroupDistanceParent;

    private Image imageIndicatorGroupDistance;
    
    [Range(0.5f, 6f)]
    [SerializeField] float updateTime = 1.5f;
    [SerializeField] private Sprite onePlayerSprite;
    [SerializeField] private Sprite twoPlayersSprite;
    [SerializeField] private Sprite threePlayersSprite;
    

    void Start()
    {
        firebaseManager = FindObjectOfType<FirebaseManager>();
        storeData = FindObjectOfType<StoreData>();
        player = FindObjectOfType<MagicPlayer>();
        
        Debug.Log(player.name);

        imageIndicatorGroupDistance = imageIndicatorGroupDistanceParent.GetComponent<Image>();


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

        StartCoroutine(DistanceBetweenPlayerCheckerCoroutine());

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

    public void ToggleNavigationPower()
    {
        Explorer ex = FindObjectOfType<Explorer>();
        ex.ToggleNavigation();
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
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.NotRenderPlayerBody();
        }
        gm.PlayerCameraObject.SetActive(false);
    }

    public IEnumerator DistanceWarningScreenSpace(string text)
    {
        distanceWarningScreenSpace.GetComponent<TMP_Text>().text = text;
        distanceWarningScreenSpace.SetActive(true);
        yield return new WaitForSeconds(2f);
        distanceWarningScreenSpace.SetActive(false);
    }

    private IEnumerator DistanceBetweenPlayerCheckerCoroutine()
    {
        while (true)
        {
            int counterPlayersClose = 0;
            
            MagicPlayer localplayer = FindObjectOfType<MagicPlayer>();
            foreach (NetworkPlayer nt in FindObjectsOfType<NetworkPlayer>())
            {
                if (!nt.isLocalPlayer)
                {
                    if (Vector3.Distance(localplayer.transform.position, nt.transform.position) < localplayer.maxDistanceFromTheOthers)
                        counterPlayersClose++;
                }
            }
            if (counterPlayersClose == 0)
            {
                imageIndicatorGroupDistance.sprite = onePlayerSprite;
            }
            else if (counterPlayersClose == 1)
            {
                imageIndicatorGroupDistance.sprite = twoPlayersSprite;
            }
            else if (counterPlayersClose == 2)
            {
                imageIndicatorGroupDistance.sprite = threePlayersSprite;
            }
            else if (counterPlayersClose > 2)
            {
                Debug.LogError("There are more then 3 players");
            }


            yield return new WaitForSeconds(updateTime);

        }
    
    }

    public void ScaleGroupSprite()
    {
        imageIndicatorGroupDistanceParent.GetComponent<Animation>().Play();
    }
}