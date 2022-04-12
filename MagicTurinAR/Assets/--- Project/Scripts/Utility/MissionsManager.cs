using System.Collections;
using System.Collections.Generic;
using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine;
using MirrorBasics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using NetworkPlayer = MirrorBasics.NetworkPlayer;
using TMPro;
using Mapbox.Unity.Map;
using Mapbox.Examples;

public class MissionsManager : MonoBehaviour
{
    public MagicTurinMissionsOrder magicTurinLevels;

    public MissionSO currentMission;
    public int currentMissionIndex;
    private NetworkPlayer networkPlayer;
    private MagicPlayer magicPlayer;
    private GameManager gameManager;
    private SpawnOnMap_Custom _spawnOnMapCustom;

    [Header("WindowFinishLevel")]
    public Canvas windowFinishLevel;
    public Text textFinishLevel;
    

    [Header("WindowStartLevel")]
    public Canvas windowToStartMission;
    public Text textWndowStartLevel;

    [Header("ExplorerPrefabs")]
    public GameObject parentTarget;
    public Vector3 offsetTargetExplorer;


    private void Start()
    {
        currentMission = magicTurinLevels.missions[currentMissionIndex];
        NetworkPlayer[] networkplayers = FindObjectsOfType<NetworkPlayer>();
        gameManager = FindObjectOfType<GameManager>();
        foreach (NetworkPlayer nt in networkplayers)
        {
            if (nt.isLocalPlayer)
            {
                networkPlayer = nt;
            }
        }
        magicPlayer = networkPlayer.GetComponent<MagicPlayer>();
        StartMission();
    }
    public void ChangeLevel()
    {
        currentMissionIndex++;
        
        if (magicTurinLevels.missions.Count < currentMissionIndex + 1)
        {
            if (magicPlayer is Wiseman)
            {
                foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                {

                    if (np.isLocalPlayer)
                        np.CmdStatsMatch(); 
                }

            }
            
        }


        currentMission = magicTurinLevels.missions[currentMissionIndex];
    }

    public void GoToStatsOfTheMatch()
    {
        SceneManager.UnloadSceneAsync("Game_Main");
        SceneManager.LoadSceneAsync("Stats", LoadSceneMode.Additive);

    }

    public void StartMission()
    {
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (magicPlayer is Explorer)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                    
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                }
                break;

            case MissionSO.PlayerType.All:
                if (magicPlayer is Wiseman)
                {
                    //Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    Vibration.Vibrate();
                    textWndowStartLevel.text = currentMission.textBeginMission;

                }

                break;
        }
    }

    public void ConfirmStartMission()
    {
        CloseStartMissionWindow();
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (magicPlayer is Explorer)
                {
                    
                    _spawnOnMapCustom = FindObjectOfType<AbstractMap>().GetComponent<SpawnOnMap_Custom>();

                    _spawnOnMapCustom.SetNewTargetLocation(currentMission.latitudeArea, currentMission.longitudeArea, currentMission.latitudeTarget, currentMission.longitudeTarget,offsetTargetExplorer);
                    
                    // Initialization for the target location of navigation power
                    //Explorer ex = FindObjectOfType<Explorer>();
                    //Debug.Log(ex.name);
                    
                    //ex.SetMissionTarget(currentMission.goalExplorerMissionPrefab.gameObject.transform);
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    gameManager.DisableMainGame();
                    foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.NotRenderPlayerBody();
                    }
                    gameManager.PlayerCameraObject.SetActive(false);
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    gameManager.DisableMainGame();
                    foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.NotRenderPlayerBody();
                    }
                    gameManager.PlayerCameraObject.SetActive(false);
                    gameManager.SetIsMission(true);
                }
                break;
            case MissionSO.PlayerType.All:
                if (magicPlayer is Wiseman)
                {
                    foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    {

                        if (np.isLocalPlayer)
                            np.CmdSharedMission();
                    }

                }
                


                break;
        }
    }


    public void BeginSharedMission()
    {
        SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
        gameManager.DisableMainGame();
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            np.NotRenderPlayerBody();
        }
        gameManager.PlayerCameraObject.SetActive(false);
    }
    public void FinishMission()
    {
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (magicPlayer is Explorer)
                {
                    //Destroy(areaTargetExplorerPrefab);
                    //Destroy(targetExplorerPrefab);

                    _spawnOnMapCustom = FindObjectOfType<AbstractMap>().GetComponent<SpawnOnMap_Custom>();
                    _spawnOnMapCustom.DestroyTargetLocation();
                    foreach (Transform t in parentTarget.GetComponentsInChildren<Transform>())
                    {
                        if(t.gameObject.name != parentTarget.gameObject.name)
                            Destroy(t.gameObject);
                    }
                    _spawnOnMapCustom.SetNavigationPower(magicPlayer.gameObject.transform);

                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.RenderPlayerBody();
                    }
                    SceneManager.UnloadSceneAsync(currentMission.sceneName);
                    gameManager.PlayerCameraObject.SetActive(true);
                    gameManager.EnableMainGame();

                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    {
                        np.RenderPlayerBody();
                    }
                    SceneManager.UnloadSceneAsync(currentMission.sceneName);
                    gameManager.PlayerCameraObject.SetActive(true);
                    gameManager.EnableMainGame();
                    gameManager.SetIsMission(false);
                }
                break;
        }
        CloseFinishMissionWindow();
        
        
        
        networkPlayer.CmdBeginNextMission();
        
        
    
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
