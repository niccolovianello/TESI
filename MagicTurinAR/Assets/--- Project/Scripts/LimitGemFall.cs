using Niantic.ARDKExamples.Helpers;
using UnityEngine;


public class LimitGemFall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        if (other.GetComponentInParent<AR_Gem>())
        {
            FindObjectOfType<UIDestroyGem>().OpenRestartWindow();
            FindObjectOfType<PlaceDynamicARObject>().LetTouch = false;
        }
    }
}
