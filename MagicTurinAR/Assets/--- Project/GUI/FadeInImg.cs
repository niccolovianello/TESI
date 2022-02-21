using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FadeInImg : MonoBehaviour
{

    private Image _logo;

    private void Awake()
    {
        _logo = GetComponent<Image>();
        _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, 0);
    }

    public IEnumerator FadeTextToFullAlpha(float t, Image i)
    {
        yield return new WaitForSeconds(1f);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeTextToZeroAlpha(t, i));
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, Image i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }
    }
}