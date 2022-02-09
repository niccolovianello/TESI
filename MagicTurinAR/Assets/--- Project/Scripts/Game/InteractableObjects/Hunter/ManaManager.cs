using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaManager : MonoBehaviour
{
    // [SerializeField] private Hunter hunter;
    [SerializeField] private Slider slider;
    [SerializeField] private float maxMana;
    private float currentMana;

    public float CurrentHealth
    {
        get => currentMana;
        set => currentMana = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        currentMana = maxMana;
        slider.maxValue = maxMana;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = currentMana;
    }

    public void DecreaseMana(float damage)
    {
        currentMana -= damage;

        if (currentMana <= 0)
        {
            // TODO
            // esci dalla missione
            // noob
        }
    }
    
    public void IncreaseMana(float increment)
    {
        currentMana += increment;
    }
}
