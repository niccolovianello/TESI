using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationInitializer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vibration.Init();
        DontDestroyOnLoad(this.gameObject);
        
    }

   
}
