using System;
using System.Collections;
using System.Collections.Generic;
using Mapbox.Map;
using UnityEngine;

public class Explorer : MagicPlayer
{


   

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


  

    
}
