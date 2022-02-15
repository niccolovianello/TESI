using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGoalScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMazeMovement>())
        {
            Debug.Log("Hai Vinto!");
        }
    }
    
}
