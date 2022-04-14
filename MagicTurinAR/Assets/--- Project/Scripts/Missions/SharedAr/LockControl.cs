using System;
using UnityEngine;

public class LockControl : MonoBehaviour
{

    public static event Action Unlocked = delegate { };
    private int[] result, correctCombination;

    private void OnEnable()
    {
        Unlocked += TriggerAnimation;
    }

    private void OnDisable()
    {
        Unlocked -= TriggerAnimation;
    }

    private void Start()
    {
        result = new [] { 7, 7, 7, 7, 7 };
        correctCombination = new [] {8, 7, 7, 7, 7};
        
        if (FindObjectOfType<MissionsManager>())
        {
            correctCombination = FindObjectOfType<MissionsManager>().currentMission.finalCode;
        }
        
        Rotate.Rotated += CheckResults;
    }


    private void CheckResults(string wheelName, int number)
    {

        switch (wheelName)
        {
            case ("Round1"):
                result[0] = number;
                break;
            case ("Round2"):
                result[1] = number;
                break;
            case ("Round3"):
                result[2] = number;
                break;
            case ("Round4"):
                result[3] = number;
                break;
            case ("Round5"):
                result[4] = number;
                break;
        }

        if (result[0] == correctCombination[0] && result[1] == correctCombination[1] && result[2] == correctCombination[2] && result[3] == correctCombination[3] && result[4] == correctCombination[4])
        {
            Unlocked();
        }

    }

    private void TriggerAnimation()
    {
        var anim = GetComponent<Animator>();
        anim.SetTrigger("Unlock");
        Destroy(gameObject, 2f);
    }



    
    
    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }

}
