using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using UnityEngine;

[RequireComponent(typeof(ManaManager))]
public class Explorer : MagicPlayer
{

    private float powerCost = .1f;

    private void Start()
    {
        manaManager = GetComponent<ManaManager>();
        manaManager.SetMaxMana(maxMana);
    }

    private void Update()
    {
        if (_uiManager.GetPower.activeSelf)
        {
            if (HasMana())
            {
                DecreaseMana(powerCost);
            }

            else
            {
                GameObject directionMesh = GameObject.Find("direction waypoint " + " entity");
                directionMesh.Destroy();
                _uiManager.TogglePower();
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



}
