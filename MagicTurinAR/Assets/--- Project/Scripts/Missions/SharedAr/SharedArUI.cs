using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharedArUI : MonoBehaviour
{
    public float waitTime = 2f;


    private void Start()
    {
        LockControl.Unlocked += FinishSharedAr;
    }
    public void OpenFinishShareArScene()
    {
        FindObjectOfType<MissionsManager>().OpenFinishMissionWindow();
        Vibration.Vibrate();
    }

    public void FinishSharedAr()
    {
        StartCoroutine(OpenCanvasSharedARCoroutine());
    }

    private IEnumerator OpenCanvasSharedARCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        OpenFinishShareArScene();
    }

    private void OnDestroy()
    {
        LockControl.Unlocked -= FinishSharedAr;
    }

}
