using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;
using UnityEngine.Events;
using Mapbox.Unity.Location;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class SessionManager : MonoBehaviour
{

    [Range(0.5f, 3.0f)]
    [SerializeField] float timeToUpdateGeoLocation = 1.5f;

    private bool flagInitialization = true;
    private NetworkPlayer networkPlayer;
    private DeviceLocationProvider deviceLocationProvider = null;

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
        StartCoroutine(FindDeviceLocationProvider());
    }


    void Update()
    {
        //if (Input.location.isEnabledByUser && flagInitialization)
        //{
        //    on_GPS_Initialized.Invoke();
        //    flagInitialization = false;
        //}

        if (!flagInitialization && deviceLocationProvider != null)
        {
            on_GPS_Initialized.Invoke();
            flagInitialization = true;
        }
        
    }

    public void StartSendGeoLocation()
    {
        StartCoroutine(SendGeoLocationToServer(timeToUpdateGeoLocation));
    }
    //public IEnumerator SendGeoLocationToServer(float timeToUpdate)
    //{

    //    while (true)
    //    {

    //        Debug.Log("Location latitude: " + Input.location.lastData.latitude + "\n Location longitude: " + Input.location.lastData.longitude + "\n Accuracy: "+ Input.location.lastData.horizontalAccuracy);
    //        networkPlayer.CmdSendGeoPositionToServer(Input.location.lastData.latitude, Input.location.lastData.latitude, networkPlayer.netId);


    //        yield return new WaitForSeconds(timeToUpdate);
    //    }

    //}



    //public IEnumerator CheckInitializationGeoLocation()
    //{
    //    while (flagInitialization)
    //    {
    //        Input.location.Start(0.5f, 3);
    //        if (Input.location.status != LocationServiceStatus.Initializing && Input.location.status != LocationServiceStatus.Failed)
    //        {
    //            flagInitialization = false;
    //            yield break;
    //        }
    //        yield return new WaitForSeconds(2.5f);


    //    }

    //}

    public IEnumerator SendGeoLocationToServer(float timeToUpdate)
    {
        while (true)      
        {

            Debug.Log("Location latitude: " + deviceLocationProvider.CurrentLocation.LatitudeLongitude.x + "\n Location longitude: " + deviceLocationProvider.CurrentLocation.LatitudeLongitude.y + "\n Accuracy: " + deviceLocationProvider.CurrentLocation.Accuracy);
            networkPlayer.CmdSendGeoPositionToServer((float)deviceLocationProvider.CurrentLocation.LatitudeLongitude.x, (float)deviceLocationProvider.CurrentLocation.LatitudeLongitude.y, networkPlayer.netId);
            yield return new WaitForSeconds(timeToUpdate);
        
        
        }

    }

    public IEnumerator FindDeviceLocationProvider()
    {
        while (flagInitialization)
        {
            deviceLocationProvider = FindObjectOfType<DeviceLocationProvider>();
            if (deviceLocationProvider != null)
            {
                flagInitialization = false;
                yield break;
            }

            yield return new WaitForSeconds(1);
                

        }
    }

   
}
