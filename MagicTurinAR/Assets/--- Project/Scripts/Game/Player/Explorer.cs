using Mapbox.Unity.MeshGeneration.Factories;
using Mapbox.Unity.MeshGeneration.Modifiers;
using UnityEngine;

[RequireComponent(typeof(ManaManager))]
public class Explorer : MagicPlayer
{
    private const float PowerCost = .2f;
    private DirectionsFactory _directions;
    private GameObject _directionMesh;
    public GameObject parentDirections;
    private bool _wasActive = false;

    private Material _directionMaterial;
    private MeshModifier _directionsMeshModifier;

    private void Start()
    {
        _directionMaterial = ItemAssets.Instance.directionMaterial;
        _directionsMeshModifier = ItemAssets.Instance.meshModifier;
        parentDirections = ItemAssets.Instance.DirectionsParent;
        
        manaManager = GetComponent<ManaManager>();
        manaManager.SetMaxMana(maxMana);
        manaManager.SetMana(maxMana);
        
        Debug.Log(maxMana);

        InitializeNavigationPower();
       
    }

    public DirectionsFactory GetDirections()
    {
        return _directions;
    }

    private void Update()
    {

        if (parentDirections.gameObject.activeSelf && parentDirections != null)
        {
         
            
            if (HasMana())
            {
                DecreaseMana(PowerCost);
            }

            else
            {
                    
                if (_directionMesh != null)
                    parentDirections.SetActive(false);
                ToggleNavigation();
                _wasActive = false;
                

            }
            if (_wasActive == false)
                _wasActive = true;
        }
        else if (!parentDirections.gameObject.activeSelf && _wasActive) 
        {
            
            if (_directionMesh != null)
                parentDirections.SetActive(false);
            _wasActive = false;
        }


    }


    private bool HasMana()
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
    
    private void DecreaseMana(float cost)
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
        var mm = FindObjectOfType<MissionsManager>();

        if (mm.currentMission.playerType == MissionSO.PlayerType.Explorer)
        {
            parentDirections.gameObject.SetActive(!parentDirections.gameObject.activeSelf);
        }

    }

    private void InitializeNavigationPower()
    {

        _directions = new GameObject("Directions").AddComponent<DirectionsFactory>();
        
        var explorerTransform = transform;
        _directions._waypoints[0] = explorerTransform;
        _directions._waypoints[1] = explorerTransform ;
        
        _directions.GetComponent<DirectionsFactory>().SetMeshModifier(_directionsMeshModifier);
        _directions.GetComponent<DirectionsFactory>().SetDirectionMaterial(_directionMaterial);
       

        //directions.gameObject.SetActive(false);
        parentDirections.SetActive(false);


    }

    public void SetMissionTarget(Transform missionTargetTransform)
    {

        _directions._waypoints[1] = missionTargetTransform;
        Debug.Log(_directions._waypoints[1]);

        _directions.gameObject.SetActive(false);

    }


}
