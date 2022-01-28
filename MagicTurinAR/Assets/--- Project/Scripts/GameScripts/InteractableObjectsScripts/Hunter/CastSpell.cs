using UnityEngine;

public class CastSpell : MonoBehaviour
{
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private Spell spellToCast;
    
    public Spell_ScriptableObject SpellSO;

    private float force;
    
    private float torque;
    
    public void Cast(float timer)
    {
        force = CalculateForce(timer);
        torque = Random.Range(1, 3);

        Spell spell = Instantiate(spellToCast, spellCastPoint.position, spellCastPoint.rotation);
        spell.SetSpellToCast(SpellSO);
        //spell.SetForce(force);

        spell.Cast(force);
    }
    
    
    private float CalculateForce(float timer)
    {
        float maxForceHoldDownTime = 2f;
        float holdTimeNormalized = Mathf.Clamp01(timer / maxForceHoldDownTime);
        float forceValue = holdTimeNormalized * SpellSO.GetMaxForce();

        return forceValue;
    }
    

    
}
