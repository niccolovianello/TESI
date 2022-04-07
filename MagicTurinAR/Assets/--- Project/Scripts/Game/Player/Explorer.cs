using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(ManaManager))]
public class Explorer : MagicPlayer
{
    private float powerCost = .1f;
    private DirectionsFactory directions;
    private GameObject directionMesh;
    public GameObject parentDirections;
    private bool wasActive = false;

    private Material directionMaterial;
    private MeshModifier directionsMeshModifier;

    private void Start()
    {
        directionMaterial = ItemAssets.Instance.directionMaterial;
        directionsMeshModifier = ItemAssets.Instance.meshModifier;
        parentDirections = ItemAssets.Instance.DirectionsParent;
        
        manaManager = GetComponent<ManaManager>();
        manaManager.SetMaxMana(maxMana);
        manaManager.SetMana(maxMana);
        
        Debug.Log(maxMana);

        InitializeNavigationPower();
       
    }

    public DirectionsFactory GetDirections()
    {
        return directions;
    }

    private void Update()
    {

        if (parentDirections.gameObject.activeSelf)
        {
         
            
            if (HasMana())
            {
                DecreaseMana(powerCost);
            }

            else
            {
                    
                if (directionMesh != null)
                    parentDirections.SetActive(false);
                ToggleNavigation();
                wasActive = false;
                

            }
            if (wasActive == false)
                wasActive = true;
        }
        else if (!parentDirections.gameObject.activeSelf && wasActive == true) 
        {
            
            if (directionMesh != null)
                parentDirections.SetActive(false);
            wasActive = false;
        }


    }


    public bool HasMana()
    {
        return currentMana > 0;
    }
    
    public void IncreaseMana(float increment)
    {
        currentMana += increment;

        if (currentMana > 100)
        {
            currentMana = 100;
        }
        
        manaManager.SetMana(currentMana);
    }
    
    public void DecreaseMana(float cost)
    {
        currentMana -= cost;

        if (currentMana < 0)
        {
            currentMana = 0;
        }
        
        manaManager.SetMana(currentMana);
    }
    
    
    public void ToggleNavigation()
    {
        MissionsManager mm = FindObjectOfType<MissionsManager>();

        if (mm.currentMission.playerType == MissionSO.PlayerType.Explorer)
        {
            parentDirections.gameObject.SetActive(!parentDirections.gameObject.activeSelf);
        }

    }

    public void InitializeNavigationPower()
    {

        directions = new GameObject("Directions").AddComponent<DirectionsFactory>();
        directions._waypoints[0] = transform;
        directions._waypoints[1] = transform ;
        
        directions.GetComponent<DirectionsFactory>().SetMeshModifier(directionsMeshModifier);
        directions.GetComponent<DirectionsFactory>().SetDirectionMaterial(directionMaterial);
       

        //directions.gameObject.SetActive(false);
        parentDirections.SetActive(false);


    }

    public void SetMissionTarget(Transform missionTargetTransform)
    {

        directions._waypoints[1] = missionTargetTransform;
        Debug.Log(directions._waypoints[1]);

        directions.gameObject.SetActive(false);

    }


}
