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
    public TMP_Text textFinishLevel;
    

    [Header("WindowStartLevel")]
    public Canvas windowToStartMission;
    public TMP_Text textWndowStartLevel;

    [Header("ExplorerPrefabs")]
    private GameObject areaTargetExplorerPrefab;
    private GameObject targetExplorerPrefab;
    private DirectionsFactory directions;
    public GameObject parentTarget;


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
        currentMission = magicTurinLevels.missions[currentMissionIndex];
    }

    public void StartMission()
    {
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (magicPlayer is Explorer)
                {
                    Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                    
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    Debug.Log("StartMission!");
                    OpenStartMissionWindow();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    Debug.Log("StartMission!");
                    OpenStartMissionWindow();
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
                    /*
                    Vector3 coordinateAreaTarget = new Vector3(currentMission.latitudeArea, 0, currentMission.longitudeArea);
                    Vector3 coordinateTarget = new Vector3(currentMission.latitudeTarget, 1.5f, currentMission.longitudeTarget);
                    areaTargetExplorerPrefab = Instantiate(currentMission.goalExplorerMissionAreaPrefab, coordinateAreaTarget, Quaternion.identity);
                    targetExplorerPrefab = Instantiate(currentMission.goalExplorerMissionPrefab, coordinateTarget, Quaternion.identity)
                    
                    
                    Debug.Log(targetExplorerPrefab.transform.position);
                    
                    */
                    
                    //_spawnOnMapCustom = FindObjectOfType<AbstractMap>().GetComponent<SpawnOnMap_Custom>();

                    //_spawnOnMapCustom.SetNewTargetLocation(currentMission.latitudeArea, currentMission.longitudeArea, currentMission.latitudeTarget, currentMission.longitudeTarget);

                    // Initialization for the target location of navigation power
                    Explorer ex = FindObjectOfType<Explorer>();
                    //ex.InitializeNavigationPower(currentMission.goalExplorerMissionPrefab.gameObject.transform);
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    gameManager.DisableMainGame();
                    networkPlayer.NotRenderPlayerBody();
                    gameManager.PlayerCameraObject.SetActive(false);
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    gameManager.DisableMainGame();
                    networkPlayer.NotRenderPlayerBody();
                    gameManager.PlayerCameraObject.SetActive(false);
                    gameManager.SetIsMission(true);
                }
                break;
        }
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
                        Destroy(t.gameObject);
                    }

                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    networkPlayer.RenderPlayerBody();
                    SceneManager.UnloadSceneAsync(currentMission.sceneName);
                    gameManager.PlayerCameraObject.SetActive(true);
                    gameManager.EnableMainGame();

                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    networkPlayer.RenderPlayerBody();
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

    public void OpenStartMissionWindow()
    {
        windowToStartMission.enabled = true;
    }

    public void CloseStartMissionWindow()
    {
        windowToStartMission.enabled = false;
    }

    public void OpenFinishMissionWindow()
    {
        windowFinishLevel.enabled = true;
        textFinishLevel.text = currentMission.textFinishMission;
    }

    public void CloseFinishMissionWindow()
    {
        windowFinishLevel.enabled = false;
    }
}
