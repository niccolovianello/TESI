using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : MagicPlayer
{
    [SerializeField] private float maxHealth;

    private float currentHealth;


    public float CurrentHealth => currentHealth;

    public void DecreaseHealth()
    {
        currentHealth -= 10;
    }
}
