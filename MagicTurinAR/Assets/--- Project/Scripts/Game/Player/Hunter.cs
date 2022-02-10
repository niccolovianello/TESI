using UnityEngine;

[RequireComponent(typeof(ManaManager))]
[RequireComponent(typeof(HealthManager))]
public class Hunter : MagicPlayer
{

    [SerializeField] private ManaManager manaManager;
    [SerializeField] private HealthManager healthManager;
    
    private void Awake()
    {
        manaManager = GetComponent<ManaManager>();
        healthManager = GetComponent<HealthManager>();
    }

    public void IncrementWhiteEnergy(float increment)
    {
        manaManager.IncreaseMana(increment);

        if (manaManager.CurrentMana > 100)
        {
            manaManager.CurrentMana = 100;
        }
        
        Debug.Log("Hunter:" + manaManager.CurrentMana);
    }
    
}
