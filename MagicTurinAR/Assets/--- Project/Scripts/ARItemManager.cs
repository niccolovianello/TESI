using System.Collections;
using System.Collections.Generic;
using Niantic.ARDKExamples.Helpers;
using UnityEngine;

public class ARItemManager : MonoBehaviour
{
    private CustomARHitTest arHitTester;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("Loader");
        GameManager gm = gameManager.GetComponent<GameManager>();

        arHitTester = GetComponent<CustomARHitTest>();
        arHitTester.PlacementObjectPf = gm.PrefabToShowInAr;
    }

}
