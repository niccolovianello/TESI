using UnityEngine;
using UnityEngine.SceneManagement;

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

    public float coordinateXExplorerMissionArea;
    public float coordinateZExplorerMissionArea;
    public GameObject goalExporerMissionAreaPrefab;

    [Header("TargetInfo")]
    public float coordinateXExplorerMission;
    public float coordinateZExplorerMission;
    public GameObject goalExporerMissionPrefab;
  



}
    
