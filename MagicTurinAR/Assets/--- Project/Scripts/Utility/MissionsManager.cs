using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using NetworkPlayer = MirrorBasics.NetworkPlayer;
using TMPro;

public class MissionsManager : MonoBehaviour
{
    public MagicTurinMissionsOrder magicTurinLevels;

    public MissionSO currentMission;
    public int currentMissionIndex;
    private NetworkPlayer networkPlayer;
    private MagicPlayer magicPlayer;
    private GameManager gameManager;

    [Header("WindowFinishLevel")]
    public Canvas windowFinishLevel;
    public TMP_Text textFinishLevel;
    

    [Header("WindowStartLevel")]
    public Canvas windowToStartMission;
    public TMP_Text textWndowStartLevel;

    [Header("ExplorerPrefabs")]
    public GameObject areaTargetExplorerPrefab;
    public GameObject targetExplorerPrefab;


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
    }
    public void ChangeLevel()
    {
        currentMissionIndex++;
        currentMission = magicTurinLevels.missions[currentMissionIndex];
        

    }

    public void StartMission()
    {
        ChangeLevel();

        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (magicPlayer is Explorer)
                {
                    OpenStartMissionWindow();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                    
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    OpenStartMissionWindow();
                    textWndowStartLevel.text = currentMission.textBeginMission;
                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
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
                    Vector3 coordinateAreaTarget = new Vector3(currentMission.coordinateXExplorerMissionArea, 0, currentMission.coordinateZExplorerMissionArea);
                    Vector3 coordinateTarget = new Vector3(currentMission.coordinateXExplorerMission, 0, currentMission.coordinateZExplorerMission);
                    areaTargetExplorerPrefab = Instantiate(currentMission.goalExporerMissionPrefab, coordinateAreaTarget, Quaternion.identity);
                    targetExplorerPrefab = Instantiate(currentMission.goalExporerMissionPrefab, coordinateTarget, Quaternion.identity);
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    gameManager.DisableMainGame();
                    magicPlayer.NotRenderPlayerBody();
                    gameManager.networkPlayerCamera.enabled = false;
                    gameManager.audioListener.enabled = false;

                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    SceneManager.LoadSceneAsync(currentMission.sceneName, LoadSceneMode.Additive);
                    gameManager.DisableMainGame();
                    magicPlayer.NotRenderPlayerBody();
                    gameManager.networkPlayerCamera.enabled = false;
                    gameManager.audioListener.enabled = false;
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
                    Destroy(areaTargetExplorerPrefab);
                    Destroy(targetExplorerPrefab);
                    
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {
                    magicPlayer.RenderPlayerBody();
                    SceneManager.UnloadSceneAsync(currentMission.sceneName);
                    gameManager.networkPlayerCamera.enabled = true;
                    gameManager.audioListener.enabled = true;
                    gameManager.EnableMainGame();

                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {
                    magicPlayer.RenderPlayerBody();
                    SceneManager.UnloadSceneAsync(currentMission.sceneName);
                    gameManager.networkPlayerCamera.enabled = true;
                    gameManager.audioListener.enabled = true;
                    gameManager.EnableMainGame();
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
