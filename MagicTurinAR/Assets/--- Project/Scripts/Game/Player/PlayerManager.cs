using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerManager : NetworkBehaviour
{
    private void Start()
    {
        SwitchDeviceSetUpForPlayerLocalization();
    }

    public void SwitchDeviceSetUpForPlayerLocalization()
    {

        #if UNITY_EDITOR

        gameObject.AddComponent<NetworkTransform>();
        Debug.Log("PlayMode");


#endif

#if UNITY_EDITOR

        Debug.Log("Android");


        #endif
    }
}
