using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class MissionsManager : MonoBehaviour
{
    public MagicTurinMissionsOrder magicTurinLevels;

    public MissionSO currentMission;
    public int currentMissionIndex;
    private NetworkPlayer networkPlayer;
    private MagicPlayer magicPlayer;
    public Canvas windowForNextLevel;
    public Canvas windowToStartMission;


    private void Start()
    {
        currentMission = magicTurinLevels.missions[currentMissionIndex];
        NetworkPlayer[] networkplayers = FindObjectsOfType<NetworkPlayer>();
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
        
        switch (currentMission.playerType)
        {
            case MissionSO.PlayerType.Explorer:
                if (magicPlayer is Explorer)
                { 
                
                }
                break;
            case MissionSO.PlayerType.Wiseman:
                if (magicPlayer is Wiseman)
                {

                }
                break;
            case MissionSO.PlayerType.Hunter:
                if (magicPlayer is Hunter)
                {

                }
                break;   
        }
    }

    public void FinishMission()
    {
        ChangeLevel();
    
    }
}
