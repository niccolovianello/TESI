using System;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Extensions;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIManagerSharedAR : MonoBehaviour
{
    public Text textHost;
    public Text textState;
    public Image panelScan;
    public Image panelChest;

    public List<String> hints = new List<String>();
    

    public void SetDebugInterfaceHost(bool isHost)
    {
        textHost.text = isHost == true ? "Leader" : "Fellow";
    }

    public void SetDebugInterfaceState(PeerStateReceivedArgs args, bool isHost)
    {
        switch (args.State)
        {
            case PeerState.WaitingForLocalizationData:
                textState.text = isHost ? "Scannerizza l'oggetto." : "Attendi che il leader completi la scansione.";
                break;
            
            case PeerState.Unknown:
                break;
            
            case PeerState.Initializing:
                break;
            
            case PeerState.Localizing:
                textState.text = isHost ? "Scannerizza l'oggetto." : "Continua a scannerizzare...";
                break;
            
            case PeerState.Stabilizing:
                break;
            
            case PeerState.Stable:
                textState.text = isHost ? "Attendi che gli altri completino la scansione." : "Scansione completata.";
                break;
            
            case PeerState.Limited:
                break;
            
            case PeerState.Failed:
                break;
            
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void EnableSharedExperience()
    {
        FindObjectOfType<ARNetworkingManager>().EnableFeatures();
        panelScan.gameObject.SetActive(false);
    }

    public void ClosePanelChest()
    {
        textState.text = "";
        textHost.text = "";
        panelChest.gameObject.SetActive(false);
        StartCoroutine(HintToUse());
    }

    public void OpenPanelChest()
    {
        panelChest.gameObject.SetActive(true);
        
    }

    private IEnumerator HintToUse()
    {
        yield return new WaitForSeconds(60f);
        
        foreach (var s in hints)
        {
            textState.text = s;
            Vibration.VibratePeek();
            
            yield return new WaitForSeconds(45f);
        }
    }

}
