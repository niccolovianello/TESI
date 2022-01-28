using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[Serializable]
public class OnLongClick : UnityEvent<float>
{
}
public class LongClickButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{

    private bool pointerDown;
    private float pointerDownTimer;

    public OnLongClick onLongClick;

    [SerializeField] private Image fillImage;
    [SerializeField] private float requiredHoldTime;

    private void Awake()
    {
        pointerDownTimer = 0;
    }


    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        float timerToPass = Mathf.Clamp(pointerDownTimer, 0, 2);
        pointerDown = false;
        onLongClick.Invoke(timerToPass);
        Reset();
    }

    private void Update()
    {
        if(pointerDown)
        {
            pointerDownTimer += Time.deltaTime;
            fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
        }
    }

    private void Reset()
    {
        pointerDown = false;
        pointerDownTimer = 0;
        fillImage.fillAmount = pointerDownTimer / requiredHoldTime;
    }
}