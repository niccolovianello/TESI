using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    private Spell_ScriptableObject SpellToCast;

    private SphereCollider collider;
    private Rigidbody rigidbody;

    //private float force;
    
    //public void SetForce(float force) => this.force = force;

    public void SetSpellToCast(Spell_ScriptableObject spellSo) => SpellToCast = spellSo;

    private void Start()
    {
        Destroy(gameObject, SpellToCast.Lifetime);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Demon demon = other.GetComponent<Demon>();
            demon.Damage(SpellToCast.Damage);
            
        }

        if (other.gameObject.CompareTag("MainCamera"))
        {
            return;
        }
        
        Destroy(gameObject);
        
    }
    

    /*
    
    private void OnCollisionEnter(Collision collision)
    {
        // Hit FX (sound, particles, ...)
        //Destroy(gameObject);
        
        Destroy(gameObject);
    }
    */
    

    public void Cast(float force, float torque)
    {
        collider = GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = SpellToCast.Radius;
        
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        
        
        rigidbody.AddForce(transform.forward * force);
        rigidbody.AddTorque(transform.right * torque);
    }

    
}
