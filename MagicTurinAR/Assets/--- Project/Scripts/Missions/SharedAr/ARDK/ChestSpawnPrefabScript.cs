using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPrefabScript : MonoBehaviour
{
    private void Start()
    {
        FindObjectOfType<SharedARManagerScript>().SetChestLocation(this.transform.position);
        
    }

   
}
