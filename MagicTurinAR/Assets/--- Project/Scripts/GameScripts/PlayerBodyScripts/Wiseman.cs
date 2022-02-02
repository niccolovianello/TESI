using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Wiseman : MagicPlayer
{
    private void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;
            
            //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);
           
            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                if (hit.collider != null)
                {

                    GameObject touchedObject = hit.transform.gameObject;

                    Debug.Log("Touched " + touchedObject.transform.name);
                }
            }
        }
    }
    [Command]
    public void SendWhiteMagic(GameObject target, int whiteMagicToSend)
    { 


    }
    
}
