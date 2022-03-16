using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vibrations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
    }

    public void VibrateFirst()
    {
        Vibration.Vibrate(50);
    }
    public void Vibrate100()
    {
        Vibration.VibrateNope();
    }
    public void Vibrate200()
    {
        Vibration.VibratePeek();
    }
    public void Vibrate400()
    {
        Vibration.Vibrate();
    }
    public void Vibrate800()
    {
        Vibration.VibratePop();
    }
}
