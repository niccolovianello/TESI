using UnityEngine;

[RequireComponent(typeof(HealthManager))]
public class Hunter : MagicPlayer
{

    [SerializeField] private HealthManager healthManager;

    private void Awake()
    {
        healthManager = GetComponent<HealthManager>();
    }

    public void DecreaseHealth(float damage)
    {
        healthManager.CurrentHealth -= damage;
    }

    // metodi player

    public void IncrementWhiteEnergy(float increment)
    {
        healthManager.IncreaseHealth(increment);

        if (healthManager.CurrentHealth > 100) healthManager.CurrentHealth = 100;
        
        Debug.Log("Hunter:" + healthManager.CurrentHealth);
    }
    
}
