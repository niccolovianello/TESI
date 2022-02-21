using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class FadeInText : MonoBehaviour
{

    private Text _text;


    private void Awake()
    {
        _text = GetComponent<Text>();
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0);
    }

    private void Start()
    {
        StartCoroutine(FadeTextToFullAlpha(2f, _text));
    }

    public IEnumerator FadeTextToFullAlpha(float t, Text i)
    {
        yield return new WaitForSeconds(2f);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / t));
            yield return null;
        }

        yield return new WaitForSeconds(3f);
        StartCoroutine(FadeTextToZeroAlpha(t, i));
    }
 
    public IEnumerator FadeTextToZeroAlpha(float t, Text i)
    {
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / t));
            yield return null;
        }

        yield return new WaitForSeconds(1f);
        GameObject.Find("StartingUI").SetActive(false);
    }
}