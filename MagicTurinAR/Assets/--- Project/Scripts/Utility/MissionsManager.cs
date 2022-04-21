using System;
using ____Project.Scripts;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using NetworkPlayer = MirrorBasics.NetworkPlayer;
using Mapbox.Unity.Map;
using Mapbox.Utils;

public class MissionsManager : MonoBehaviour
{
    public MagicTurinMissionsOrder magicTurinLevels;

    public MissionSO currentMission;
    public int currentMissionIndex;
    private NetworkPlayer _networkPlayer;
    private MagicPlayer _magicPlayer;
    private GameManager _gameManager;
    private SpawnOnMapCustom _spawnOnMapCustom;

    [Header("WindowFinishLevel")]
    public Canvas windowFinishLevel;
    public Text textFinishLevel;
    
    [Header("WindowStartLevel")]
    public Canvas windowToStartMission;
    public Text textWindowStartLevel;

    [Header("ExplorerPrefabs")]
    public GameObject parentTarget;
    public Vector3 offsetTargetExplorer;


    private void Start()
    {
        currentMission = magicTurinLevels.missions[currentMissionIndex];
        var networkPlayers = FindObjectsOfType<NetworkPlayer>();
        _gameManager = FindObjectOfType<GameManager>();
        foreach (var np in networkPlayers)
        {
            if (np.isLocalPlayer)
            {
                _networkPlayer = np;
            }
        }
        _magicPlayer = _networkPlayer.GetComponent<MagicPlayer>();
        StartMission();
    }


    public void ChangeLevel()
    {
        currentMissionIndex++;

        if (magicTurinLevels.missions.Count < currentMissionIndex + 1)
        {
            GoToStatsOfTheMatch();
        }
        else {

            currentMission = magicTurinLevels.missions[currentMissionIndex];
            StartMission();

        }
        
    }

    private static void GoToStatsOfTheMatch()
    {
        SceneManager.UnloadSceneAsync("Game_Main");
        SceneManager.LoadSceneAsync("Stats", LoadSceneMode.Additive);
    }

    private void StartMission()
    {
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (_magicPlayer is Explorer)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    Debug.Log(textWindowStartLevel + " - " + currentMission.textBeginMission);
                    textWindowStartLevel.text = currentMission.textBeginMission;
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (_magicPlayer is Wiseman)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWindowStartLevel.text = currentMission.textBeginMission;
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (_magicPlayer is Hunter)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWindowStartLevel.text = currentMission.textBeginMission;
                }
                break;

            case MissionSO.PlayerType.All:
                if (_magicPlayer is Wiseman)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWindowStartLevel.text = currentMission.textBeginMission;

                }

                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void ConfirmStartMission()
    {
        CloseStartMissionWindow();
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (_magicPlayer is Explorer)
                {
                    var map = FindObjectOfType<AbstractMap>();
                    
                    _spawnOnMapCustom = map.GetComponent<SpawnOnMapCustom>();
                    _spawnOnMapCustom.SetNewTargetLocation(currentMission.latitudeArea, currentMission.longitudeArea, currentMission.latitudeTarget, currentMission.longitudeTarget, offsetTargetExplorer);

                    var areaPos = map.GeoToWorldPosition(new Vector2d(currentMission.latitudeArea, currentMission.longitudeArea));
                    
                    var mainCamera = FindObjectOfType<Camera>().gameObject;
                    var cutSceneManager = FindObjectOfType<CutSceneManager>();
                    
                    mainCamera.SetActive(false);
                    cutSceneManager.EnableCutSceneCamera();
                    
                    cutSceneManager.MoveCamera(areaPos, mainCamera.transform.position);
                    
                    cutSceneManager.DisableCutSceneCamera();
                    mainCamera.SetActive(true);
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (_magicPlayer is Wiseman)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    _gameManager.DisableMainGame();
                    foreach (var np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.NotRenderPlayerBody();
                    }
                    _gameManager.PlayerCameraObject.SetActive(false);
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (_magicPlayer is Hunter)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    _gameManager.DisableMainGame();
                    foreach (var np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.NotRenderPlayerBody();
                    }
                    _gameManager.PlayerCameraObject.SetActive(false);
                    _gameManager.SetIsMission(true);
                }
                break;
            case MissionSO.PlayerType.All:
                if (_magicPlayer is Wiseman)
                {
                    foreach (var np in FindObjectsOfType<NetworkPlayer>())
                    {

                        if (np.isLocalPlayer)
                            np.CmdSharedMission();
                    }

                }
                


                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public void BeginSharedMission()
    {
        SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
        _gameManager.DisableMainGame();
        foreach (var np in FindObjectsOfType<NetworkPlayer>())
        {
            np.NotRenderPlayerBody();
        }
        _gameManager.PlayerCameraObject.SetActive(false);
    }
    public void FinishMission()
    {
        if (magicTurinLevels.missions.Count >= currentMissionIndex + 2)
        {

            switch (currentMission.playerType)
            {
                case MissionSO.PlayerType.Explorer:
                    if (_magicPlayer is Explorer)
                    {
                        //Destroy(areaTargetExplorerPrefab);
                        //Destroy(targetExplorerPrefab);

                        _spawnOnMapCustom = FindObjectOfType<AbstractMap>().GetComponent<SpawnOnMapCustom>();
                        _spawnOnMapCustom.DestroyTargetLocation();
                        foreach (var t in parentTarget.GetComponentsInChildren<Transform>())
                        {
                            if (t.gameObject.name != parentTarget.gameObject.name)
                                Destroy(t.gameObject);
                        }
                        SpawnOnMapCustom.SetNavigationPower(_magicPlayer.gameObject.transform);

                    }
                    break;
                case MissionSO.PlayerType.Wiseman:
                    if (_magicPlayer is Wiseman)
                    {
                        foreach (var np in FindObjectsOfType<NetworkPlayer>())
                        {
                            np.RenderPlayerBody();
                        }
                        SceneManager.UnloadSceneAsync(currentMission.sceneName);
                        _gameManager.PlayerCameraObject.SetActive(true);
                        _gameManager.EnableMainGame();

                    }
                    break;
                case MissionSO.PlayerType.Hunter:
                    if (_magicPlayer is Hunter)
                    {
                        foreach (var np in FindObjectsOfType<NetworkPlayer>())
                        {
                            np.RenderPlayerBody();
                        }
                        SceneManager.UnloadSceneAsync(currentMission.sceneName);
                        _gameManager.PlayerCameraObject.SetActive(true);
                        _gameManager.EnableMainGame();
                        _gameManager.SetIsMission(false);
                    }
                    break;

                case MissionSO.PlayerType.All:

                    foreach (var np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.RenderPlayerBody();
                    }
                    SceneManager.UnloadSceneAsync(currentMission.sceneName);
                    _gameManager.PlayerCameraObject.SetActive(true);
                    _gameManager.EnableMainGame();

                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            CloseFinishMissionWindow();
            _networkPlayer.CmdBeginNextMission();
        }

        else
        {
            SceneManager.UnloadSceneAsync(currentMission.sceneName);
            GoToStatsOfTheMatch();
        }
        
    }

    private void OpenStartMissionWindow()
    {
        windowToStartMission.enabled = true;
    }

    private void CloseStartMissionWindow()
    {
        windowToStartMission.enabled = false;
    }

    public void OpenFinishMissionWindow()
    {
        windowFinishLevel.enabled = true;
        textFinishLevel.text = currentMission.textFinishMission;
    }

    private void CloseFinishMissionWindow()
    {
        windowFinishLevel.enabled = false;
    }
}
