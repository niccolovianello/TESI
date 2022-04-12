using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControl : MonoBehaviour
{

    private int[] result, correctCombination;

    void Start()
    {
        result = new int[] { 5, 5, 5, 5, 5 };
        correctCombination = FindObjectOfType<MissionsManager>().currentMission.finalCode;
        Rotate.Rotated += CheckResults;
    }


    private void CheckResults(string wheelName, int number)
    {

        switch (wheelName)
        {
            case ("Wheel1"):
                result[0] = number;
                break;
            case ("Wheel2"):
                result[1] = number;
                break;
            case ("Wheel3"):
                result[2] = number;
                break;
            case ("Wheel4"):
                result[3] = number;
                break;
            case ("Wheel5"):
                result[4] = number;
                break;
        }

        if (result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2] && result[3] == correctCombination[3] && result[4] == correctCombination[4])
        {
            Debug.Log("Opened!!");
        }

    }

    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;

    }

}
