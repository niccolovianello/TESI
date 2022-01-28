using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using MirrorBasics;
using Mapbox.Examples;
using Mirror;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private MagicPlayer currentPlayer;

    public GameObject parentObjPlayer;
    public GameObject parentObjItems;
    public GameObject parentObjEnemies;
    public LobbyNetworkPlayer networkPlayer;
    public Camera networkPlayerCamera;
    public AudioListener audioListener;
    public MagicPlayer CurrentPlayer => currentPlayer;
    public GameObject mainGame;
    private GameObject prefabToShowInAR;
    UnityEvent playerEnterInGameEvent;



    public GameObject PrefabToShowInAr => prefabToShowInAR;
    public void SetPrefabToShowInAR(GameObject prefab) => prefabToShowInAR = prefab;
    public void DisableMainGame()
    {
        mainGame.SetActive(false);

        //    parentObjPlayer = GameObject.Find("/ParentPlayer");
        //    parentObjItems = GameObject.Find("ParentItems");
        //    parentObjEnemies = GameObject.Find("ParentEnemies");
        //}

    }
    public void EnableMainGame()
    {
        mainGame.SetActive(true);
    }
    private void Awake()

    {
        if (playerEnterInGameEvent == null)
            playerEnterInGameEvent = new UnityEvent();

        playerEnterInGameEvent.AddListener(SetUpPlayer);
    }
    private void Start()
    {
        currentPlayer = FindObjectOfType<MagicPlayer>();
        LobbyNetworkPlayer[] listOfPlayer = FindObjectsOfType<LobbyNetworkPlayer>();

        foreach (LobbyNetworkPlayer player in listOfPlayer)
        {
            if (player.isLocalPlayer)
            {
                networkPlayer = player;
                networkPlayerCamera = player.GetComponentInChildren<Camera>();
                audioListener = player.GetComponentInChildren<AudioListener>();
                //networkPlayer.GetComponent<NetworkTransform>().clientAuthority = true;
                playerEnterInGameEvent.Invoke();
                
                break;
            }
        }

        foreach (var camera in FindObjectsOfType<Camera>())
        {

            if (camera.tag == "External Camera")
            {
                camera.enabled = false;
                camera.GetComponent<AudioListener>().enabled = false;

            }
            

        }



    }

  
    void SetUpPlayer()
    {
        Item[] items;
        networkPlayerCamera.enabled = true;
        audioListener.enabled = true;
        
        //networkPlayer.gameObject.AddComponent<ImmediatePositionWithLocationProvider>();
        //networkPlayer.gameObject.AddComponent<RotateWithLocationProvider>();
        
        networkPlayer.gameObject.AddComponent<Explorer>();
        
        //networkPlayer.gameObject.AddComponent<ControllerMovement>();
        
        networkPlayerCamera.gameObject.AddComponent<GemFactory>();
        networkPlayerCamera.gameObject.AddComponent<WhiteFragmentFactory>();
        networkPlayerCamera.gameObject.AddComponent<EnemyFactory>();

       


        ItemAssets itemAssets = FindObjectOfType<ItemAssets>();

    
        GemFactory gemFactory = networkPlayerCamera.GetComponent<GemFactory>();
        gemFactory.SetItemPrefab(itemAssets.gem);
        gemFactory.waitTime = 3;
        gemFactory.startingItems = 3;
        gemFactory.minRange = 0;
        gemFactory.maxRange = 10;


        WhiteFragmentFactory whiteFragmentFactory = networkPlayerCamera.GetComponent<WhiteFragmentFactory>();
        whiteFragmentFactory.SetItemPrefab(itemAssets.whiteFragment);
        whiteFragmentFactory.waitTime = 6;
        whiteFragmentFactory.startingItems = 1;
        whiteFragmentFactory.minRange = 0;
        whiteFragmentFactory.maxRange = 15;


        EnemyFactory enemyFactory = networkPlayerCamera.GetComponent<EnemyFactory>();
        enemyFactory.SetEnemyPrefab(itemAssets.enemy);


        Explorer explorer = networkPlayer.GetComponent<Explorer>();

        items = FindObjectsOfType<MagicItem>();

        explorer.SetUIManager(FindObjectOfType<UIManager>());
        
        foreach (MagicItem item in items)
        {
            item.player = networkPlayer.GetComponent<Explorer>();
            item.magicPlayer = networkPlayer.GetComponent<Explorer>();
        }

        //networkPlayer.gameObject.transform.parent = parentObjPlayer.transform;

    }


}
