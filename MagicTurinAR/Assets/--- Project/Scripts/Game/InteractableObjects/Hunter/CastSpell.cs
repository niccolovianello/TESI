using System.Collections;
using TMPro;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private Spell spellToCast;
    [SerializeField] private Spell_ScriptableObject SpellSO;
    [SerializeField] private TMP_Text manaWarning;
    

    private float force;
    private float torque;
    private ManaManager manaManager;


    public void Cast(float timer)
    {
        manaManager = FindObjectOfType<ManaManager>();

        if (manaManager.CurrentMana < SpellSO.Cost)
        {
            StartCoroutine(ManaWarning());
        }
        
        else
        {
            force = CalculateForce(timer);
            torque = Random.Range(1, 3);

            Spell spell = Instantiate(spellToCast, spellCastPoint.position, spellCastPoint.rotation);
            spell.SetSpellToCast(SpellSO);

            spell.Cast(force, torque);
        
            manaManager.DecreaseMana(SpellSO.Cost);
        }
    }
    
    
    private float CalculateForce(float timer)
    {
        float maxForceHoldDownTime = 1f;
        float holdTimeNormalized = Mathf.Clamp01(timer / maxForceHoldDownTime);
        float forceValue = holdTimeNormalized * SpellSO.GetMaxForce();

        return forceValue;
    }

    private IEnumerator ManaWarning()
    {
        manaWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        manaWarning.gameObject.SetActive(false);
    }
    

    
}
