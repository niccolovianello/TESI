using System;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerSharedAR : MonoBehaviour
{
    public Text textHost;
    public Text textState;

    public void SetDebugInterfaceHost(string hostString)
    {
        textHost.text = hostString == "true" ? "Leader" : "Fellow";
    }

    public void SetDebugInterfaceState(PeerStateReceivedArgs args, bool isHost)
    {
        switch (args.State)
        {
            case PeerState.WaitingForLocalizationData:
                textState.text = isHost ? "Scan an object." : "Wait for the leader to complete the scanning process.";
                break;
            
            case PeerState.Unknown:
                break;
            
            case PeerState.Initializing:
                break;
            
            case PeerState.Localizing:
                textState.text = isHost ? "Scan an object." : "Keep scanning.";
                break;
            
            case PeerState.Stabilizing:
                break;
            
            case PeerState.Stable:
                textState.text = isHost ? "Wait for the others to complete the scanning process." : "Scan completed.";
                break;
            
            case PeerState.Limited:
                break;
            
            case PeerState.Failed:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    
}
