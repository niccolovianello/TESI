using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Wiseman : MagicPlayer
{
    [SerializeField] private int whiteMagicToSend = 100;
    private void Update()
    {
        //if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        //{
        //    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
        //    RaycastHit hit;

        //    //Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow, 100f);

        //    if (Physics.Raycast(ray, out hit))
        //    {
        //        Debug.Log(hit.transform.name);
        //        if (hit.collider != null)
        //        {

        //            GameObject touchedObject = hit.transform.gameObject;

        //            Debug.Log("Touched " + touchedObject.transform.name);
        //        }
        //    }
        //}

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                if (hit.collider != null)
                {

                    GameObject touchedObject = hit.transform.gameObject;

                    if (touchedObject.tag == "Player")
                    {
                        Debug.Log(this);
                        networkPlayer.SendWhiteMagic(touchedObject, whiteMagicToSend);
                        uiInventory.textGems.text = (Int32.Parse(uiInventory.textGems.text) - 1).ToString();
                        foreach (MagicItemSO item in ItemAssets.Instance.magicInventorySO.items)
                        {
                            if (item.id == 2000) // White Fragment specific code
                            {
                                item.prefab.GetComponent<MagicItem>().amount--;
                            }
                        
                        }
                    }
                }
            }

        }
    }
   
}
