using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;



[Serializable]

public abstract class Item : MonoBehaviour, ItemInterface
{
    private float maxClickDistance = 10;

    private float rotateSpeed = 50.0f;
    private float floatAmplitude = 0.005f;
    private float floatFrequency = 0.5f;

    //[SerializeField] public Canvas canvasItem;
    //[SerializeField] public TMP_Text textWarning;

    public UIInventory uiInventory;

    public Player player;
    public GameObject itemGO;
    [SerializeField] public int amount ;

    
    public abstract void OnMouseDown();
    
    public abstract void RenderItem(int typeRole);

    
    void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;

        transform.position = tempPos;
    }


    public bool IsClickable()
    {
        return (player.transform.position - transform.position).magnitude < maxClickDistance;
    }


    
    
}
