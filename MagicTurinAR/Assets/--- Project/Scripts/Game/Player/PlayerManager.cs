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

        if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            Debug.Log("PlayMode");

        }

        //Check if the device running this is a handheld
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            Debug.Log("Android");
            Destroy(gameObject.GetComponent<NetworkTransform>());
        }

    }
}
