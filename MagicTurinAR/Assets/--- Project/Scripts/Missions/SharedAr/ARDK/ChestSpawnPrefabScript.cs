using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSpawnPrefabScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<SharedARManagerScript>().SetChestLocation(this.transform.position);
    }

   
}
