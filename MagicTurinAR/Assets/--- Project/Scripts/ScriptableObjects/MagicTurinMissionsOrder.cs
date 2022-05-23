using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewMissionsOrder", menuName = "MagicItems/MagicMissionsOrder")]
public class MagicTurinMissionsOrder : ScriptableObject
{
    public List<MissionSO> missions = new List<MissionSO>();

}
