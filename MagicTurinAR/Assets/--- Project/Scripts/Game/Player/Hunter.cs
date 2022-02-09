using UnityEngine;

[RequireComponent(typeof(ManaManager))]
public class Hunter : MagicPlayer
{

    [SerializeField] private ManaManager manaManager;

    private void Awake()
    {
        manaManager = GetComponent<ManaManager>();
    }

    public void DecreaseHealth(float damage)
    {
        manaManager.CurrentHealth -= damage;
    }

    // metodi player

    public void IncrementWhiteEnergy(float increment)
    {
        manaManager.IncreaseMana(increment);

        if (manaManager.CurrentHealth > 100) manaManager.CurrentHealth = 100;
        
        Debug.Log("Hunter:" + manaManager.CurrentHealth);
    }
    
}
