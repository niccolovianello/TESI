using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicInventory", menuName = "MagicItems/MagicInventory")]
public class MagicInventorySO : ScriptableObject
{
    public List<MagicItemSO> items = new List<MagicItemSO>();
}
