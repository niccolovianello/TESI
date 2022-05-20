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
using Niantic.ARDKExamples.Helpers;


public class SharedARManagerScript : MonoBehaviour
{
    public GameObject PoseIndicator;
    public Vector3 offsetPoseIndicator = new Vector3(0f, 1.0f, 0);
    private IARNetworking _arNetworking;
    private IMultipeerNetworking _networking;
    private IARSession _session;

    [SerializeField]
    private FeaturePreloadManager preloadManager = null;

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
    public bool _isSynced;
    private bool _objectsSpawned = false;

    private Dictionary<IPeer, GameObject> _playerIndcator = new Dictionary<IPeer, GameObject>();

    public int stablePeerCount = 0;

    private bool chestLocationSet;




    private void Awake()
    {
        ARNetworkingFactory.ARNetworkingInitialized += OnInitialized;
        preloadManager.ProgressUpdated += PreloadProgressUpdated;
    }

    public bool IsHost
    {
        get => _isHost;
    }

    public bool ChestLocationSet
    {
        get => chestLocationSet;
        set => chestLocationSet = value;
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

    private void Start()
    {
        FindObjectOfType<ARNetworkingManager>().EnableFeatures();
    }
    private void Update()
    {
        //if(Input.GetKeyDown(KeyCode.A))
       
        // start game conditions
        if ( _isGameStarted == false && stablePeerCount == 3 && chestLocationSet)
        {
            StartGame();
            if (_isHost)
                FindObjectOfType<CameraManagerSharedAR>().DestroyNotHostComponents();
           
        }
    }

    public void SetChestLocation(Vector3 position)
    {
        if (_chest != null)
        {
            if (!_isGameStarted)
                _isGameStarted = true;

            _chest.transform.position = position;
        }

        else
        {
            _location = position;
        }
            
        chestLocationSet = true;
        
        Debug.Log("Position of spawn prefab set: " + _location);
    }

    public void StartGame()
    {
        if(_isHost)
            FindObjectOfType<ChestSpawnPrefabScript>().gameObject.Destroy();
        Debug.Log("Start Game");
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
        
        _chest.transform.Rotate(new Vector3(0, 180, 0), Space.World);
        if(_isHost)
            Debug.Log(_chest.transform.position);

        _chestBehviour = _chest.GetComponent<Chest>();
        _messagingManager.SetChestReference(_chestBehviour);
        _chestBehviour.Controller = this;

        _messagingManager.BroadCastChestLocationSet(true);
        _objectsSpawned = true;

        Debug.Log("BeforeCmdSpawn");
        if (!_isHost)
            return;

        _messagingManager.SpawnGameObjects(position);

        Debug.Log("AfterCmdSpawn");
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

        if (!_isHost)
        {
            FindObjectOfType<CameraManagerSharedAR>().DestroyNotHostComponents();
        }

        FindObjectOfType<UIManagerSharedAR>().SetDebugInterfaceHost(_isHost.ToString());
        Debug.LogFormat("Peer connected: {0}, isHost: {1}", args.Self.Identifier.ToString(), args.IsHost);
        Debug.Log("is host: " + _isHost);
    }

    public void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        if (args.State == PeerState.Stable)
            stablePeerCount++;
        Debug.LogFormat("State:  {0} Peer: {1}", args.State, args.Peer);
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
        FindObjectOfType<UIManagerSharedAR>().SetDebugInterfaceState(args.State.ToString());


    }

    public void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {

        if (!_playerIndcator.ContainsKey(args.Peer))
        {
            _playerIndcator.Add(args.Peer, Instantiate(PoseIndicator));
            //Debug.Log("Add Indicator");
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




