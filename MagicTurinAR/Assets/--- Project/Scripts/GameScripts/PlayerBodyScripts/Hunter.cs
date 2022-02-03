using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MagicPlayer
{
    [SerializeField] private int whitePower = 0;
    [SerializeField] private int whiteMagicForSpecialAttack = 50;

    [SerializeField] private float maxHealth;

    private float currentHealth;


    public float CurrentHealth => currentHealth;

    public void DecreaseHealth()
    {
        currentHealth -= 10;
    }

    public void DecreaseWhiteMagicPower()
    {
        whitePower -= whiteMagicForSpecialAttack;
        
    }
    
    // metodi player

    public void IncrementWhiteEnergy(int increment)
    {
        whitePower += increment;

        if (whitePower > 100) whitePower = 100;
        
        Debug.Log("Hunter:" + whitePower);
    }
}
