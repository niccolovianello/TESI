using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMagicItem", menuName = "MagicItems/MagicItem")]
public class MagicItemSO : ScriptableObject
{
    public int id;
 
    public ItemType itemType;

    public enum ItemType
    { 
        Artifact,
        Book,
        Rune,
        Gem,
        WhiteFragment,
        
    }

    public bool isStackable;

    public string description;
    public Sprite Sprite;
    public GameObject prefab;

    
}
