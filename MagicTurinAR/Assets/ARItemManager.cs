using System.Collections;
using System.Collections.Generic;
using Niantic.ARDKExamples.Helpers;
using UnityEngine;

public class ARItemManager : MonoBehaviour
{
    private ARHitTester arHitTester;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("Loader");
        GameManager gm = gameManager.GetComponent<GameManager>();

        arHitTester = GetComponent<ARHitTester>();
        arHitTester.PlacementObjectPf = gm.PrefabToShowInAr;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
