using Mapbox.Directions;
using Mapbox.Examples;
using Mapbox.Unity.Map;
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
    private bool initializationFlag = false;

    private void Start()
    {

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
                Debug.Log("Activate");
            }

            else
            {
                    
                if (directionMesh != null)
                    parentDirections.SetActive(false);
                ToggleNavigation();
                wasActive = false;
                Debug.Log("Finish Mana");
                

            }
            if (wasActive == false)
                wasActive = true;
        }
        else if (!parentDirections.gameObject.activeSelf && wasActive == true) 
        {
            
            if (directionMesh != null)
                parentDirections.SetActive(false);
            wasActive = false;
            Debug.Log(" Deactivate");
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

        //directions.gameObject.SetActive(!directions.gameObject.activeSelf);
        parentDirections.gameObject.SetActive(!parentDirections.gameObject.activeSelf);

        

        
           


    }

    public void InitializeNavigationPower()
    {

        directions = new GameObject("Directions").AddComponent<DirectionsFactory>();
        //directions.transform.parent = parentDirections.transform;
        directions._waypoints[0] = transform;
        directions._waypoints[1] = transform ;

        MeshModifier mm = (MeshModifier)AssetDatabase.LoadAssetAtPath("Assets/Mapbox/Examples/1_DataExplorer/Traffic/DirectionLoft.asset", typeof(MeshModifier));
        directions.GetComponent<DirectionsFactory>().SetMeshModifier(mm);

        Material mat = (Material)AssetDatabase.LoadAssetAtPath("Assets/Mapbox/Examples/Resources/DirectionMaterial.mat", typeof(Material));
        directions.GetComponent<DirectionsFactory>().SetDirectionMaterial(mat);
       

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
