using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewMission", menuName = "MagicItems/Mission")]
public class MissionSO : ScriptableObject
{
    public int id;
    public PlayerType playerType;

    public enum PlayerType
    {
        Explorer,
        Wiseman,
        Hunter,
        All
    }

    public string sceneName;
    public string textBeginMission;
    public string textFinishMission;
    public bool missionCompleted;


    [Header("HunterMission")]
    public float demonVelocity;

    [Header("TargetAreaInfo")]
    public float latitudeArea;
    public float longitudeArea;
    
    
    public GameObject goalExplorerMissionAreaPrefab;

    [Header("TargetInfo")]
    public float latitudeTarget;
    public float longitudeTarget;
    public GameObject goalExplorerMissionPrefab;

    [Header("SecretCode For Shared Ar")]

    public int[] finalCode;

}
    
