using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    
    private Renderer[] _renderers;
    private void OnEnable()
    {
        LockControl.Unlocked += TriggerAnimation;
    }

    private void Start()
    {
        _renderers = GetComponents<Renderer>();
    }

    private void TriggerAnimation()
    {
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float alpha = 0.95f;

        do
        {
            foreach (var r in _renderers)
            {
                r.material.SetFloat("_Alpha", alpha);
            }
            
            alpha -= 0.025f;
            yield return new WaitForSeconds(.025f);
            
        } while (alpha > 0f);
    }
}
