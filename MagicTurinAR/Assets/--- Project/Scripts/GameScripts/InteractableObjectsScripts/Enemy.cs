using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class Enemy : MonoBehaviour
{
    private CapsuleCollider collider;
    
    private void Awake()
    {
        collider = GetComponent<CapsuleCollider>();
        collider.isTrigger = true;
    }
    
}
