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
        Vibration.Vibrate(100);
    }
    public void Vibrate200()
    {
        Vibration.Vibrate(200);
    }
    public void Vibrate400()
    {
        Vibration.Vibrate(400);
    }
    public void Vibrate800()
    {
        Vibration.Vibrate(800);
    }
}
