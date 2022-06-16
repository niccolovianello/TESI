using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DebugManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (EventSystem ev in FindObjectsOfType<EventSystem>())
            {
                Debug.Log(ev.gameObject.name);
            }
        }
    }
}
