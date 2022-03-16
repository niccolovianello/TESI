using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGoalScript : MonoBehaviour
{
    private PlayerMazeMovement mp;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerMazeMovement>())
        {
            mp = FindObjectOfType<PlayerMazeMovement>();
            Destroy(mp.gameObject);            
            FindObjectOfType<MissionsManager>().OpenFinishMissionWindow();
            Vibration.Vibrate();
            Destroy(gameObject);
        }
    }
    
}
