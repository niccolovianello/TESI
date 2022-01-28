using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Demon : Enemy
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private Text life;
    private float currentHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        life.text = currentHealth.ToString();
    }

    public void Damage(float damage)
    {
        currentHealth -= damage;
        
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    
}
