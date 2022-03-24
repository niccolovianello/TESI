using Mapbox.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonsForTouchControl : MonoBehaviour
{
    [Header("Buttons' Sprites")]
    public Sprite lockSprite;
    public Sprite freeNavigationSprite;
    public Sprite freeRotation;
    public Sprite automaticRotation;

    [Header("Buttons")]
    public Button lockOrFreeNavigationButton;
    public Button automaticOrFreeRotation;

    private RotateWithLocationProvider rotationProvider;


    public void ChangeButtonSpriteInLock()
    {
        if (lockOrFreeNavigationButton.image.sprite == lockSprite)
        {
            lockOrFreeNavigationButton.image.sprite = freeNavigationSprite;
        }
        else {
            lockOrFreeNavigationButton.image.sprite = lockSprite;
        }
        
    }

    public void SetRotationProvider(RotateWithLocationProvider rp)
    {
        rotationProvider = rp;
    }

    public void ChangeButtonSpriteRotation()
    {
      
        
        if (automaticOrFreeRotation.image.sprite == freeRotation)
        {
            rotationProvider.enabled = true;
            automaticOrFreeRotation.image.sprite = automaticRotation;
        }
        else
        {
            rotationProvider.enabled = false;
            automaticOrFreeRotation.image.sprite = freeRotation;
        }
    }
}
