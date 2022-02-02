using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using UnityEngine;

public class Explorer : MagicPlayer
{
    private int whitePower = 100;


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



}
