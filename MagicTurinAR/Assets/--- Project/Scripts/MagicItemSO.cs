using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMagicItem", menuName = "MagicItems/MagicItem")]
public class MagicItemSO : ScriptableObject
{
    public int id;
 
    public int itemTypeIndex;

    public bool isStackable;

    public string description;
    public Sprite Sprite;
    public GameObject prefab;

    
}
