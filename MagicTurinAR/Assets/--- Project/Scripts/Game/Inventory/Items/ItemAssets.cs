using Mapbox.Unity.MeshGeneration.Modifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemAssets : MonoBehaviour
{
    public static ItemAssets Instance {get; private set;}
    public MagicInventorySO magicInventorySO;

    private void Awake()
    { 
        Instance = this;
    }
    
    public Transform prefabItemCollectable;

    [Header("Enemy Prefab")]
   
    public GameObject enemy;

    [Header("Gem Prefab")]
   
    public GameObject gem;

    [Header("WhiteFragment Prefab")]

    public GameObject whiteFragment;

    [Header("Character Prefabs")]
    public GameObject explorerPrefab;
    public GameObject hunterPrefab;
    public GameObject wisemanPrefab;

    [Header("Parents Transform")]
    public GameObject DirectionsParent;

    [Header("RadarVFX")]
    public GameObject prefabRadarVfx;

    [Header("DirectionPowerUtilities")]
    public MeshModifier meshModifier;
    public Material directionMaterial;


    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        foreach (EventSystem e in FindObjectsOfType<EventSystem>())
    //        {
    //            Debug.Log(e.name);
    //        }
    //    }
    //}

}
