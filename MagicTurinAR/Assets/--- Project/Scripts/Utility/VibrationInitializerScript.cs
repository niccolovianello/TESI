using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationInitializerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
        DontDestroyOnLoad(this.gameObject);
        
    }

   
}
