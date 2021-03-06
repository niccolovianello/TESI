using UnityEngine;

[CreateAssetMenu(fileName = "NewMission", menuName = "MagicItems/Mission")]
public class MissionSO : ScriptableObject
{
    public int id;
    public PlayerType playerType;

    public enum PlayerType
    {
        Explorer,
        Wiseman,
        Hunter
    }

    public string sceneName;
    public string textBeginMission;
    public string textFinishMission;
    public bool missionCompleted;


    [Header("TargetAreaInfo")]
    public float latitudeArea;
    public float longitudeArea;
    
    
    public GameObject goalExplorerMissionAreaPrefab;

    [Header("TargetInfo")]
    public float latitudeTarget;
    public float longitudeTarget;
    public GameObject goalExplorerMissionPrefab;

}
    
