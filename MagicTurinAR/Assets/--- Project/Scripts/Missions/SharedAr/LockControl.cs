using System;
using System.Collections;
using UnityEngine;

public class LockControl : MonoBehaviour
{

    public static event Action Unlocked = delegate { };
    [SerializeField] private float timeToOpenFinishMissionWindow = 2f;

    private int[] result, correctCombination;



    private void Start()
    {
      
        result = new[] { 7, 7, 7, 7, 7 };
        correctCombination = new[] { 8, 3, 8, 4, 2};

        if (FindObjectOfType<MissionsManager>())
        {
            correctCombination = FindObjectOfType<MissionsManager>().currentMission.finalCode;
        }

        Rotate.Rotated += CheckResults;
    }


    private void OnEnable()
    {
        Unlocked += TriggerAnimation;

    }

    private void OnDisable()
    {
        Unlocked -= TriggerAnimation;

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
        StartCoroutine(FinishLevel());
        Destroy(gameObject, timeToOpenFinishMissionWindow + 0.5f);
    }

    private IEnumerator FinishLevel()
    {
        yield return new WaitForSeconds(timeToOpenFinishMissionWindow);
        if (FindObjectOfType<MissionsManager>())
        {
            FindObjectOfType<MissionsManager>().OpenFinishMissionWindow();
        }
    }



    
    
    private void OnDestroy()
    {
        Rotate.Rotated -= CheckResults;
    }

}
