using Mapbox.Directions;
using Mapbox.Examples;
using Mapbox.Unity.Map;
using Mapbox.Unity.MeshGeneration.Factories;
using UnityEngine;

[RequireComponent(typeof(ManaManager))]
public class Explorer : MagicPlayer
{
    
    [SerializeField] private Transform targetArea;

    private float powerCost = .1f;
    private DirectionsFactory directions;
    private SpawnOnMap target;
    

    private MissionsManager _missionsManager;
    


    private void Awake()
    {
        _missionsManager = FindObjectOfType<MissionsManager>();
        directions = FindObjectOfType<DirectionsFactory>();
        target = FindObjectOfType<AbstractMap>().GetComponent<SpawnOnMap>();
    }
    
    private void Start()
    {
        target.SetNewTargetLocation(targetArea, _missionsManager.currentMission.latitude, _missionsManager.currentMission.longitude);
        directions._waypoints[0] = transform;
        directions._waypoints[1] = targetArea;
        directions.gameObject.SetActive(false);
        manaManager = GetComponent<ManaManager>();
        manaManager.SetMaxMana(maxMana);
    }

    private void Update()
    {
        if (directions.gameObject.activeSelf)
        {
            if (HasMana())
            {
                DecreaseMana(powerCost);
            }

            else
            {
                GameObject directionMesh = GameObject.Find("direction waypoint " + " entity");
                directionMesh.Destroy();
                ToggleNavigation();
            }
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
        directions.gameObject.SetActive(!directions.gameObject.activeSelf);
    }


}
