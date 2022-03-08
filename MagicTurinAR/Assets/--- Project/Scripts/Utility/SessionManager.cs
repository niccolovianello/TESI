using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;
using UnityEngine.Events;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class SessionManager : MonoBehaviour
{

    [Range(0.5f, 3.0f)]
    [SerializeField] float timeToUpdateGeoLocation = 1.5f;

    private bool flagInitialization = true;
    private NetworkPlayer networkPlayer;

    UnityEvent on_GPS_Initialized;


    public void Start()
    {
        foreach(NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
        {
            if(np.isLocalPlayer)
                networkPlayer = np;
        }

        if (on_GPS_Initialized == null)
            on_GPS_Initialized = new UnityEvent();

        
        on_GPS_Initialized.AddListener(StartSendGeoLocation);
    }


    void Update()
    {
        if (Input.location.isEnabledByUser && flagInitialization)
        {
            on_GPS_Initialized.Invoke();
            flagInitialization = false;
        }
    }

    public void StartSendGeoLocation()
    {
        StartCoroutine(SendGeoLocationToServer(timeToUpdateGeoLocation));
    }
    public IEnumerator SendGeoLocationToServer(float timeToUpdate)
    {

        while (true)
        {
            Debug.Log("Location latitude: " + Input.location.lastData.latitude + "\n Location longitude: " + Input.location.lastData.longitude);
            networkPlayer.CmdSendGeoPositionToServer(Input.location.lastData.latitude, Input.location.lastData.latitude, networkPlayer.netId);
            yield return new WaitForSeconds(timeToUpdate);
        }

    }

   

    //public IEnumerator CheckInitializationGeoLocation()
    //{
    //    while (flagInitialization)
    //    {
    //        if (Input.location.status != LocationServiceStatus.Initializing && Input.location.status != LocationServiceStatus.Failed)
    //        {
    //            flagInitialization = false;
    //        }
    //        yield return new WaitForSeconds(0.5f);


    //    }
         
    //}
}
