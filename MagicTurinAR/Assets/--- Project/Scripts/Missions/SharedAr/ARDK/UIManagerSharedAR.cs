using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerSharedAR : MonoBehaviour
{
    public Text textHost;
    public Text textState;

    public void SetDebugInterfaceHost(string hostString)
    {
        textHost.text = "isHost: " + hostString;
    }

    public void SetDebugInterfaceState(string peerStateString)
    {
        textState.text = "State: " + peerStateString;
    }
}
