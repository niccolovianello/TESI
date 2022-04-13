using UnityEngine;

public class Chest : MonoBehaviour
{
    private void OnEnable()
    {
        LockControl.Unlocked += TriggerAnimation;
    }
    
    private void TriggerAnimation()
    {
        GetComponent<Animator>().SetTrigger("Unlock");
    }
}
