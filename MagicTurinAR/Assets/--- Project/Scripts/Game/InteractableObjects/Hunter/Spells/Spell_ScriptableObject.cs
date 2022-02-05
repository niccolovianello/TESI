using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]

public class Spell_ScriptableObject : ScriptableObject
{
    public float Cost = 5f;
    public float Lifetime = 2f;
    public float Damage = 10f;
    public float Radius = .5f;
    
    public const float MAX_FORCE = 2000f;

    public float GetMaxForce() => MAX_FORCE;

}
