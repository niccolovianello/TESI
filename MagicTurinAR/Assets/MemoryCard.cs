using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MemoryCard : MonoBehaviour
{

    private BoxCollider collider;
    private Animation animation;

    private void Start()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;

        animation = GetComponent<Animation>();
    }

    private void OnMouseDown()
    {
        animation.Play();
    }
}
