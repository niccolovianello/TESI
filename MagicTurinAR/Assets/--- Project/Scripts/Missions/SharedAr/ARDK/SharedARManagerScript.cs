using System.Collections.Generic;
using UnityEngine;


using Niantic.ARDK.AR;
using Niantic.ARDK.AR.ARSessionEventArgs;
using Niantic.ARDK.AR.Configuration;
using Niantic.ARDK.AR.Networking;
using Niantic.ARDK.AR.Networking.ARNetworkingEventArgs;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.MultipeerNetworkingEventArgs;
using Niantic.ARDK.Utilities;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking.HLAPI;
using Niantic.ARDK.Networking.HLAPI.Authority;
using Niantic.ARDK.Networking.HLAPI.Object;
using System.IO;
using Niantic.ARDK.Utilities.BinarySerialization;

public class SharedARManagerScript : MonoBehaviour
{
    public GameObject PoseIndicator;
    public Vector3 offsetPoseIndicator = new Vector3(-.5f, 0, 0);
    private IARNetworking _arNetworking;
    private IMultipeerNetworking _networking;
    private IARSession _session;
    private FeaturePreloadManager preloadManager;

    [Header("Gameplay Prefabs")]
    [SerializeField]
    private GameObject chest = null;

    private Vector3 _location;

    private GameObject _chest = null;
    private Chest _chestBehviour;
    private MessagingManagerChestQuest _messagingManager;

    private IPeer _host;
    private IPeer _self;
    private bool _isHost;

    private bool _isGameStarted = false;
    private bool _isSynced;
    private bool _objectsSpawned = false;


    private Dictionary<IPeer, GameObject> _playerIndcator = new Dictionary<IPeer, GameObject>();


    private void Start()
    {
        ARNetworkingFactory.ARNetworkingInitialized += OnInitialized;
        preloadManager.ProgressUpdated += PreloadProgressUpdated;
    }

    private void PreloadProgressUpdated(FeaturePreloadManager.PreloadProgressUpdatedArgs args)
    {
        if (args.PreloadAttemptFinished)
        {
            if (args.FailedPreloads.Count > 0)
            {
                Debug.LogError("Failed to download resources needed to run AR Multiplayer");
                return;
            }

            preloadManager.ProgressUpdated -= PreloadProgressUpdated;
        }
    }

    private void Update()
    {
        // start game conditions
        if (_playerIndcator.Values.Count == 3 && _isGameStarted == false)
        {
            StartGame();
           
        }
    }

    public void SetChestLocation(Vector3 position)
    {
        if (!_isGameStarted)
            _isGameStarted = true;

        _chest.transform.position = position;
    }

    public void StartGame()
    {
        if (!_objectsSpawned)
        {
            InstantiateObjects(_location);
        }
            
        _isGameStarted = true;
        _chestBehviour.GameStart(_isHost, _messagingManager);
    }

    internal void InstantiateObjects(Vector3 position)
    {
        if (_chest != null)
        {
            Debug.Log("Relocating the playing field!");
            chest.transform.position = position;

            if (_isHost)
                _messagingManager.SpawnGameObjects(position);

            return;
        }

        _chest = Instantiate(chest, position, Quaternion.identity);

        _chestBehviour = _chest.GetComponent<Chest>();
        _messagingManager.SetChestReference(_chestBehviour);
        _chestBehviour.Controller = this;

        _objectsSpawned = true;


        if (!_isHost)
            return;

        _messagingManager.SpawnGameObjects(position);


    }

        //public void CreateAndRunSharedAR()
        //{
        //    // create Mock in Editor, LiceDevice on mobile
        //    _arNetworking = ARNetworkingFactory.Create();

        //    _networking = _arNetworking.Networking;
        //    _session = _arNetworking.ARSession;

        //    // configuration of ar session

        //    var worldTrackingConfig = ARWorldTrackingConfigurationFactory.Create();
        //    worldTrackingConfig.WorldAlignment = WorldAlignment.Gravity;
        //    worldTrackingConfig.IsAutoFocusEnabled = true;

        //    // set CV synchro pipeline

        //    worldTrackingConfig.IsSharedExperienceEnabled = true;

        //    //run session, listen the event
        //    _session.Run(worldTrackingConfig);
        //    _session.Ran += OnSessionRan;

        //    var sessionID = "abcd";
        //    var sessionIdAsByte = System.Text.Encoding.UTF8.GetBytes(sessionID);

        //    //Join the networkSession
        //    _networking.Join(sessionIdAsByte);

        //    // Listening Network Events
        //    _networking.Connected += OnNetworkConnected;

        //    _arNetworking.PeerStateReceived += OnPeerStateReceived;
        //    _arNetworking.PeerPoseReceived += OnPeerPoseReceived;

        //    //code by us

        //    _networking.PeerDataReceived += OnPeerDataReceived;

        //}

        private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("session ran");
    }

    public void OnNetworkConnected(ConnectedArgs args)
    {
        _self = args.Self;
        _host = args.Host;
        _isHost = args.IsHost;
    }

    public void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        if (_self.Identifier == args.Peer.Identifier)
            UpdateOwnState(args);
        else
            UpdatePeerState(args);
    }

    private void UpdatePeerState(PeerStateReceivedArgs args)
    {
        if (args.State == PeerState.Stable)
        {
            _isSynced = true;

           
        }
    }

    private void UpdateOwnState(PeerStateReceivedArgs args)
    {
        string message = args.State.ToString();
        Debug.Log("We reached state " + message);
    }

    public void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        if (!_playerIndcator.ContainsKey(args.Peer))
        {
            _playerIndcator.Add(args.Peer, Instantiate(PoseIndicator));
        }

        if (_playerIndcator.TryGetValue(args.Peer, out PoseIndicator))
        {
            PoseIndicator.transform.position = args.Pose.ToPosition() + offsetPoseIndicator;
        }
    }

    private void OnInitialized(AnyARNetworkingInitializedArgs args)
    {
        _arNetworking = args.ARNetworking;
        _session = _arNetworking.ARSession;
        _networking = _arNetworking.Networking;


        //subscrition events
        _session.Ran += OnSessionRan;
        _networking.Connected += OnNetworkConnected;
        _arNetworking.PeerStateReceived += OnPeerStateReceived;
        _arNetworking.PeerPoseReceived += OnPeerPoseReceived;


        _messagingManager = new MessagingManagerChestQuest();
        _messagingManager.InitializeMessagingManager(args.ARNetworking.Networking, this);

    }

    private void OnDestroy()
    {
        ARNetworkingFactory.ARNetworkingInitialized -= OnInitialized;

        if (_arNetworking != null)
        {
            _session.Ran -= OnSessionRan;
            _networking.Connected -= OnNetworkConnected;
            _arNetworking.PeerStateReceived -= OnPeerStateReceived;
            _arNetworking.PeerPoseReceived -= OnPeerPoseReceived;
        }
        if (_messagingManager != null)
        {
            _messagingManager.Destroy();
            _messagingManager = null;
        }
    }

}




