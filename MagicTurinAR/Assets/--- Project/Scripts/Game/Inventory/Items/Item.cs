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
    
    private float maxClickDistance = 30;

    [SerializeField] private float rotateSpeed = 30.0f;
    [SerializeField] private float floatAmplitude = 0.01f;
    [SerializeField] private float floatFrequency = 0.5f;
    

    public UIInventory uiInventory;

    public Player player;
    public GameObject itemGO;
    [SerializeField] public int amount;

    public abstract void OnMouseDown();

    
    internal virtual void Update()
    {
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime);
        Vector3 tempPos = transform.position;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;

        transform.position = tempPos;
    }

    public float MaxClickDistance
    {
        get => maxClickDistance;
    }

    public bool IsClickable()
    {
        return (player.transform.position - transform.position).magnitude < maxClickDistance;
    }

    public abstract void DoNotRenderItem();
    
    public abstract void RenderItem();
    
}
