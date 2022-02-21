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
    
    [SerializeField] private Transform targetArea;

    private float powerCost = .1f;
    private DirectionsFactory directions;

    private void Awake()
    {

        directions = new GameObject("Directions").AddComponent<DirectionsFactory>();
        
        MeshModifier mm = (MeshModifier) AssetDatabase.LoadAssetAtPath("Assets/Mapbox/Examples/1_DataExplorer/Traffic/DirectionLoft.asset", typeof(MeshModifier));
        directions.GetComponent<DirectionsFactory>().SetMeshModifier(mm);
        
        Material mat = (Material) AssetDatabase.LoadAssetAtPath("Assets/Mapbox/Examples/Resources/DirectionMaterial.mat", typeof(Material));
        directions.GetComponent<DirectionsFactory>().SetDirectionMaterial(mat);
        
        directions._waypoints[0] = transform;
        directions._waypoints[1] = transform;

        //Instantiate(directions, Vector3.zero, Quaternion.identity);

        directions.gameObject.SetActive(false);

    }
    
    private void Start()
    {
       
       
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

    public void InitializeNavigationPower(Transform playerTransform, Transform targetTransform)
    {
        
        directions._waypoints[0] = playerTransform;
        directions._waypoints[1] = targetTransform;
        Debug.Log(directions._waypoints[1]);

        directions.gameObject.SetActive(false);
    }


}
