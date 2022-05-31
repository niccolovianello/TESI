using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class Spell_ScriptableObject : ScriptableObject
{
    public float Cost;
    public float Lifetime;
    public float Damage;
    public float Radius;
    
    public const float MAX_FORCE = 500f;

    public float GetMaxForce() => MAX_FORCE;

}
