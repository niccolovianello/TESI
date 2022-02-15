using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MemoryCard : MonoBehaviour
{
    [SerializeField] private int cardId;

    private BoxCollider collider;
    private Animation animation;

    private MemoryManager memoryManager;

    public int CardId => cardId;
    public Animation Animation => animation;
    public BoxCollider Collider => collider;

    private void Start()
    {
        memoryManager = FindObjectOfType<MemoryManager>();
        
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;

        animation = GetComponentInChildren<Animation>();
    }

    private void OnMouseDown()
    {
        Animation.Play("Scoprire");
        StartCoroutine(CheckTime());
    }
    
    private IEnumerator CheckTime()
    {
        yield return new WaitForSeconds(2f);
        memoryManager.CheckMatch(this);
    }

    
}
