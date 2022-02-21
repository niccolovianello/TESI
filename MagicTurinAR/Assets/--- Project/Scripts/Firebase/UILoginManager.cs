using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILoginManager : MonoBehaviour
{

    public static UILoginManager instance;

    //Screen object variables
    public GameObject startingUI;
    public GameObject loginUI;
    public GameObject registerUI;

    private Image _logo;
    private Text _subTitle;
    private Image _over;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
        
        _logo = startingUI.GetComponentInChildren<Image>();
        _subTitle = startingUI.GetComponentInChildren<Text>();
        
        _logo.color = new Color(_logo.color.r, _logo.color.g, _logo.color.b, 0);
        _subTitle.color = new Color(_subTitle.color.r, _subTitle.color.g, _subTitle.color.b, 0);
        
        _over = GameObject.Find("Over").GetComponent<Image>();
        
    }

    private void Start()
    {
        StartCoroutine(FadeToFullAlpha(2f, 1f, 2f, _logo));
        StartCoroutine(FadeToFullAlpha(2f, 1.5f, 2f, _subTitle));
    }

    //Functions to change the login screen UI
    public void LoginScreen() // Back button
    {
        loginUI.SetActive(true);
        registerUI.SetActive(false);
    }
    public void RegisterScreen() // Register button
    {
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }
    
    private IEnumerator FadeToFullAlpha(float duration, float waitTimeBeforeStart, float waitTimeAfterFinish, MaskableGraphic i)
    {
        yield return new WaitForSeconds(waitTimeBeforeStart);
        while (i.color.a < 1.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a + (Time.deltaTime / duration));
            yield return null;
        }

        yield return new WaitForSeconds(waitTimeAfterFinish);
        StartCoroutine(FadeToZeroAlpha(duration, 0f, 1f, i));
    }
 
    private IEnumerator FadeToZeroAlpha(float duration, float waitTimeBeforeStart, float waitTimeAfterFinish, MaskableGraphic i)
    {
        yield return new WaitForSeconds(waitTimeBeforeStart);
        i.color = new Color(i.color.r, i.color.g, i.color.b, 1);
        while (i.color.a > 0.0f)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, i.color.a - (Time.deltaTime / duration));
            yield return null;
        }

        yield return new WaitForSeconds(waitTimeAfterFinish);

        if (i is Text)
        {
            startingUI.SetActive(false);
        }

        StartCoroutine(LoginUIFadeIn(duration));
    }

    private IEnumerator LoginUIFadeIn(float duration)
    {
        yield return new WaitForSeconds(1f);
        while (_over.color.a > 0.0f)
        {
            _over.color = new Color(_over.color.r, _over.color.g, _over.color.b, _over.color.a - (Time.deltaTime / duration));
            yield return null;
        }
        
        yield return new WaitForSeconds(1f);
        _over.gameObject.SetActive(false);
    }
}

