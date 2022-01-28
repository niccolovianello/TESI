using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ControllerMovement : NetworkBehaviour
{
    
    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer)
            return;

        if (Input.GetKey(KeyCode.W))
        {
            this.gameObject.transform.position +=  new Vector3(1, 0, 0);
        }
    }
}
