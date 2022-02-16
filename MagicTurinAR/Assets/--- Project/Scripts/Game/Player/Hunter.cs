using UnityEngine;

[RequireComponent(typeof(ManaManager))]
public class Hunter : MagicPlayer
{

    private void Start()
    {
        manaManager = GetComponent<ManaManager>();
        manaManager.SetMaxMana(maxMana);
        manaManager.SetMana(currentMana);
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
    
}
