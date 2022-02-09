using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{

    // [SerializeField] private Hunter hunter;
    [SerializeField] private Slider slider;
    [SerializeField] private float maxHealth;
    private float currentHealth;

    public float CurrentHealth
    {
        get => currentHealth;
        set => currentHealth = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        slider.maxValue = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = currentHealth;
    }

    public void DecreaseHealth(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // esci dalla missione
            // noob
        }
    }
    
    public void IncreaseHealth(float increment)
    {
        currentHealth += increment;
    }
}
