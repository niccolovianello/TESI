using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LimitGemFallScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.GetComponent<MeshRenderer>())
        {
            
            
            FindObjectOfType<UIDestroyGem>().OpenRestartWindow();
        }
    }
}
