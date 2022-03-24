using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using MirrorBasics;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.Location;
using Mirror;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private MagicPlayer currentPlayer;

    private bool isMission = false;

    public GameObject touchControlManager;
    public NetworkPlayer networkPlayer;
    public Camera networkPlayerCamera;
    public AudioListener audioListener;
    public MagicPlayer CurrentPlayer => currentPlayer;
    public GameObject mainGame;
    public GameObject prefabToShowInAR;
    private UIButtonsForTouchControl uiButtonsForTouchControl;
    UnityEvent playerEnterInGameEvent;



    public GameObject PlayerCameraObject= null;


    public GameObject PrefabToShowInAr => prefabToShowInAR;
    public void SetPrefabToShowInAR(GameObject prefab) => prefabToShowInAR = prefab;
    public void DisableMainGame()
    {
        mainGame.SetActive(false);
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

        LobbyNetworkPlayer[] listOfPlayer = FindObjectsOfType<NetworkPlayer>();

        foreach (NetworkPlayer player in listOfPlayer)
        {
            switch (player.TypePlayerEnum)
            {
                case NetworkPlayer.TypePlayer.Explorer:
                    Instantiate(ItemAssets.Instance.explorerPrefab, player.playerBody.transform);
                    break;
                case NetworkPlayer.TypePlayer.Wiseman:
                    Instantiate(ItemAssets.Instance.wisemanPrefab, player.playerBody.transform);
                    break;
                case NetworkPlayer.TypePlayer.Hunter:
                    Instantiate(ItemAssets.Instance.hunterPrefab, player.playerBody.transform);
                    break;
            }
               
            if (player.isLocalPlayer)
            {
                networkPlayer = player;

                networkPlayerCamera = player.GetComponentInChildren<Camera>();
                audioListener = player.GetComponentInChildren<AudioListener>();
                PlayerCameraObject = player.GetComponentInChildren<Camera>().gameObject;
                playerEnterInGameEvent.Invoke();



                uiButtonsForTouchControl = FindObjectOfType<UIButtonsForTouchControl>();
                uiButtonsForTouchControl.lockOrFreeNavigationButton.onClick.AddListener(() => FindObjectOfType<CameraMovements>().ButtonCenterCameraOnPlayer());
                uiButtonsForTouchControl.lockOrFreeNavigationButton.onClick.AddListener(() => FindObjectOfType<CameraMovements>().FreeOrAutomaticRotation());
                
            }
        }
        CameraMovements cm = FindObjectOfType<CameraMovements>();
        cm.Camera = networkPlayerCamera;
        cm.CameraFocus = networkPlayer.gameObject;

    }
    private void Start()
    {
        
        
        currentPlayer = FindObjectOfType<MagicPlayer>();
        
        
        RangeAroundTransformTileProviderOptions rangeAroundTransformTileProviderOptions = new RangeAroundTransformTileProviderOptions();
        rangeAroundTransformTileProviderOptions.SetOptions(currentPlayer.transform);
        FindObjectOfType<AbstractMap>().SetExtentOptions(rangeAroundTransformTileProviderOptions);
        

        foreach (var camera in FindObjectsOfType<Camera>())
        {

            if (camera.tag == "External Camera")
            {
                camera.enabled = false;
                camera.GetComponent<AudioListener>().enabled = false;
            }

        }

    }
<<<<<<< HEAD
=======
    
    /*
    private void Start()
    {
        
        
        

    }
    */
<<<<<<< HEAD
>>>>>>> parent of 25cf500 (start gM)
=======
>>>>>>> parent of 25cf500 (start gM)

  
    void SetUpPlayer()
    {
        Item[] items;
        networkPlayerCamera.enabled = true;
        audioListener.enabled = true;
        
        if (!networkPlayer.GetComponent<NetworkTransform>())
        {
            SessionManager sm = networkPlayer.gameObject.AddComponent<SessionManager>();
        }

        networkPlayer.gameObject.AddComponent<DeviceLocationProvider>();
        networkPlayer.gameObject.AddComponent<ImmediatePositionWithLocationProvider>();
        RotateWithLocationProvider rt = networkPlayer.gameObject.AddComponent<RotateWithLocationProvider>();
        rt.SetDeviceOrientation(true);
        FindObjectOfType<UIButtonsForTouchControl>().SetRotationProvider(rt);
                

        ItemAssets itemAssets = FindObjectOfType<ItemAssets>();
        
        switch (networkPlayer.TypePlayerEnum)
        {
            case NetworkPlayer.TypePlayer.Explorer:
                
                networkPlayerCamera.gameObject.AddComponent<GemFactory>();
                GemFactory gemFactory = networkPlayerCamera.GetComponent<GemFactory>();


                gemFactory.SetItemPrefab(itemAssets.gem);
                gemFactory.waitTime = 3;
                gemFactory.startingItems = 3;
                gemFactory.minRange = 0;
                gemFactory.maxRange = 10;


                networkPlayer.gameObject.AddComponent<Explorer>();
                Explorer explorer = networkPlayer.GetComponent<Explorer>();
                currentPlayer = explorer;
                explorer.parentDirections = ItemAssets.Instance.DirectionsParent;


                items = FindObjectsOfType<MagicItem>();

                explorer.SetUIManager(FindObjectOfType<UIManager>());

                foreach (MagicItem item in items)
                {
                    item.player = explorer;
                    item.magicPlayer = explorer;
                }

                break; 
            
            case NetworkPlayer.TypePlayer.Wiseman:
                networkPlayerCamera.gameObject.AddComponent<WhiteFragmentFactory>();
                WhiteFragmentFactory whiteFragmentFactory = networkPlayerCamera.GetComponent<WhiteFragmentFactory>();


                whiteFragmentFactory.SetItemPrefab(itemAssets.whiteFragment);
                whiteFragmentFactory.waitTime = 6;
                whiteFragmentFactory.startingItems = 1;
                whiteFragmentFactory.minRange = 0;
                whiteFragmentFactory.maxRange = 15;

                networkPlayer.gameObject.AddComponent<Wiseman>();
                Wiseman wiseman = networkPlayer.GetComponent<Wiseman>();
                currentPlayer = wiseman;

                items = FindObjectsOfType<MagicItem>();

                wiseman.SetUIManager(FindObjectOfType<UIManager>());

                foreach (MagicItem item in items)
                {
                    item.player = wiseman;
                    item.magicPlayer = wiseman;
                }
                break;
            
            case NetworkPlayer.TypePlayer.Hunter:
                networkPlayerCamera.gameObject.AddComponent<EnemyFactory>();
                EnemyFactory enemyFactory = networkPlayerCamera.GetComponent<EnemyFactory>();
                enemyFactory.SetEnemyPrefab(itemAssets.enemy);


                networkPlayer.gameObject.AddComponent<Hunter>();
                Hunter hunter = networkPlayer.GetComponent<Hunter>();
                currentPlayer = hunter;
                

                items = FindObjectsOfType<MagicItem>();

                hunter.SetUIManager(FindObjectOfType<UIManager>());

                foreach (MagicItem item in items)
                {
                    item.player = hunter;
                    item.magicPlayer = hunter;
                }
                break;
        }
        
    }



    public void NotRenderPlayerBody()
    {
        networkPlayer.NotRenderPlayerBody();
    }

    public void RenderPlayerBody()
    {
        networkPlayer.RenderPlayerBody();
    }

    public bool GetIsMission()
    {
        return isMission;
    }

    public void SetIsMission(bool _isMission)
    {
        isMission = _isMission;
    }
}

public static class TransformExtensions
{
    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag)
    {
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.tag == tag)
            {
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0)
            {
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
          
        }
        return taggedGameObjects;
    }
}
