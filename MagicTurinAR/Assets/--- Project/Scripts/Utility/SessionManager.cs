using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;
using UnityEngine.Events;
using Mapbox.Unity.Location;
using UnityEngine.UI;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class SessionManager : MonoBehaviour
{

    [Range(0.5f, 3.0f)]
    [SerializeField] float updateTime = 1.5f;

    private float bestAccuracy = 10;
    private float worstAccuracy = 50;

    private bool isInitializing = true;
    private NetworkPlayer networkPlayer;
    private DeviceLocationProvider deviceLocationProvider = null;

    private UIManager _uiManager;

    private float fill;

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

        _uiManager = FindObjectOfType<UIManager>();

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

        if (!isInitializing && deviceLocationProvider != null)
        {
            on_GPS_Initialized.Invoke();
            isInitializing = true;
        }
        
    }

    private void StartSendGeoLocation()
    {
        StartCoroutine(SendGeoLocationToServer(updateTime));
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

    private IEnumerator SendGeoLocationToServer(float updateTime)
    {
        while (true)      
        {
            networkPlayer.CmdSendGeoPositionToServer((float)deviceLocationProvider.CurrentLocation.LatitudeLongitude.x, (float)deviceLocationProvider.CurrentLocation.LatitudeLongitude.y, networkPlayer.netId);

            fill = CalculateFill(worstAccuracy, bestAccuracy, 0, 1, deviceLocationProvider.CurrentLocation.Accuracy);
            _uiManager.gpsAccuracy.fillAmount = fill;
            
            yield return new WaitForSeconds(updateTime);
        }
        
    }

    private IEnumerator FindDeviceLocationProvider()
    {
        while (isInitializing)
        {
            deviceLocationProvider = FindObjectOfType<DeviceLocationProvider>();
            if (deviceLocationProvider != null)
            {
                isInitializing = false;
                yield break;
            }

            yield return new WaitForSeconds(1);
            
        }
    }

    public float CalculateFill(float oldMin, float oldMax, float newMin, float newMax, float oldValue){
 
        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;
        float newValue = /* 1 - */ ((oldValue - oldMin) * newRange / oldRange + newMin);
 
        return newValue;
    }

   
}
