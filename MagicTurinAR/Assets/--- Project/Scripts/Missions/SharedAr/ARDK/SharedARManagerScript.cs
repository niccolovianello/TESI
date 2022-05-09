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


public class SharedARManagerScript : MonoBehaviour
{
    public GameObject PoseIndicator;
    public Vector3 offsetPoseIndicator = new Vector3(.5f, 0, 0);
    private IARNetworking _arNetworking;
    private IMultipeerNetworking _networking;
    private IARSession _session;

    private Dictionary<IPeer, GameObject> _poseIndicatorDict = new Dictionary<IPeer, GameObject>();
    public void CreateAndRunSharedAR()
    {
        // create Mock in Editor, LiceDevice on mobile
        _arNetworking = ARNetworkingFactory.Create();

        _networking = _arNetworking.Networking;
        _session = _arNetworking.ARSession;

        // configuration of ar session

        var worldTrackingConfig = ARWorldTrackingConfigurationFactory.Create();
        worldTrackingConfig.WorldAlignment = WorldAlignment.Gravity;
        worldTrackingConfig.IsAutoFocusEnabled = true;

        // set CV synchro pipeline

        worldTrackingConfig.IsSharedExperienceEnabled = true;

        //run session, listen the event
        _session.Run(worldTrackingConfig);
        _session.Ran += OnSessionRan;

        var sessionID = "abc";
        var sessionIdAsByte = System.Text.Encoding.UTF8.GetBytes(sessionID);

        //Join the networkSession
        _networking.Join(sessionIdAsByte);

        // Listening Network Events
        _networking.Connected += OnNetworkConnected;

        _arNetworking.PeerStateReceived += OnPeerStateReceived;
        _arNetworking.PeerPoseReceived += OnPeerPoseReceived;

    }

    private void OnSessionRan(ARSessionRanArgs args)
    {
        Debug.Log("session ran");
    }
    public void OnNetworkConnected(ConnectedArgs args)
    {
        Debug.LogFormat("Network joined, peerID: {0}, isHost: {1}", args.Self, args.IsHost);
    }

    public void OnPeerStateReceived(PeerStateReceivedArgs args)
    {
        Debug.LogFormat("Peer: {0} is at state: {1}", args.Peer, args.State);
    }
    public void OnPeerPoseReceived(PeerPoseReceivedArgs args)
    {
        if (!_poseIndicatorDict.ContainsKey(args.Peer))
        {
            _poseIndicatorDict.Add(args.Peer, Instantiate(PoseIndicator));
        }

        if (_poseIndicatorDict.TryGetValue(args.Peer, out PoseIndicator))
        {
            PoseIndicator.transform.position = args.Pose.ToPosition() + offset;
        }
    }
    private void OnDestroy()
    {
        _session?.Dispose();
        _networking.Dispose();
        _networking.Leave();
        _arNetworking.Dispose();
    }
}

