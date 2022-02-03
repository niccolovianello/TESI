using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using UnityEngine;

public class Explorer : MagicPlayer
{
    public int whitePower = 0;


    private void Update()
    {
        if (_uiManager.GetPower.activeSelf)
        {
            if (HasWhitePower())
            {
                ReduceWhitePower();
            }

            else
            {
                GameObject directionMesh = GameObject.Find("direction waypoint " + " entity");
                directionMesh.Destroy();
                _uiManager.TogglePower();
            }
        }
    }

    public void ReduceWhitePower()
    {
        whitePower--;
    }

    public bool HasWhitePower()
    {
        return whitePower > 0;
    }
    
    public void IncrementWhiteEnergy(int increment)
    {
        whitePower += increment;
        
        if (whitePower > 100) whitePower = 100;
        Debug.Log("Explorer:" + whitePower);
    }



}
