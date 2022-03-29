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
    private GameObject gameEnvironment;

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
        gameEnvironment = FindObjectOfType<GameManager>().mainGame;
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
    
 

    private IEnumerator SendGeoLocationToServer(float updateTime)
    {
        while (true)      
        {
            yield return new WaitForSeconds(updateTime);
            //Debug.Log("gioco attivo: "+ gameEnvironment.gameObject.activeSelf);
            if (gameEnvironment.gameObject.activeSelf)
            {
                networkPlayer.CmdSendGeoPositionToServer((float)deviceLocationProvider.CurrentLocation.LatitudeLongitude.x, (float)deviceLocationProvider.CurrentLocation.LatitudeLongitude.y, networkPlayer.netId);
                //Debug.Log("controllo posizione" );
                fill = CalculateFill(worstAccuracy, bestAccuracy, 0, 1, deviceLocationProvider.CurrentLocation.Accuracy);
                _uiManager.gpsAccuracy.fillAmount = fill;
            }
            
                
            
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

    private float CalculateFill(float oldMin, float oldMax, float newMin, float newMax, float oldValue){
 
        float oldRange = oldMax - oldMin;
        float newRange = newMax - newMin;
        float newValue = (oldValue - oldMin) * newRange / oldRange + newMin;

        if (newValue <= .25f) newValue = .25f;
        else if (newValue > .25f && newValue <= .5f) newValue = .5f;
        else if (newValue > .5f && newValue <= .75f) newValue = .75f;
        else if (newValue > .75f) newValue = 1f;
 
        return newValue;
    }

   
}
