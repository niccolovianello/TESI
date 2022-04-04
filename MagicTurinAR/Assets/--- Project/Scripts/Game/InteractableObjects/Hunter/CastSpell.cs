using System.Collections;
using TMPro;
using UnityEngine;

public class CastSpell : MonoBehaviour
{
    [SerializeField] private Transform spellCastPoint;
    [SerializeField] private Spell spellToCast;
    [SerializeField] private Spell_ScriptableObject spellSo;
    [SerializeField] private TMP_Text manaWarning;
    

    private float _force;
    private float _torque;
    private Hunter _hunter;


    public void Cast(float timer)
    {
        _hunter = FindObjectOfType<Hunter>();

        
        if (_hunter.CurrentMana < spellSo.Cost)
        {
            StartCoroutine(ManaWarning());
        }
        
        
        else
        
        {
            _force = CalculateForce(timer);
            _torque = Random.Range(1, 3);

            Spell spell = Instantiate(spellToCast, spellCastPoint.position, spellCastPoint.rotation);
            spell.SetSpellToCast(spellSo);

            spell.Cast(_force, _torque);
        
            _hunter.DecreaseMana(spellSo.Cost);
        }
    }
    
    
    private float CalculateForce(float timer)
    {
        float maxForceHoldDownTime = 1f;
        float holdTimeNormalized = Mathf.Clamp01(timer / maxForceHoldDownTime);
        float forceValue = holdTimeNormalized * spellSo.GetMaxForce();

        return forceValue;
    }

    private IEnumerator ManaWarning()
    {
        manaWarning.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        manaWarning.gameObject.SetActive(false);
    }
    
    
}
