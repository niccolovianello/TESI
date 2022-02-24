using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using Mapbox;
using System;

namespace MirrorBasics
{
    public class LobbyNetworkPlayer : NetworkBehaviour


    {
        public bool peerIsTheHost = false;

        

        [SyncVar] public string matchID;
        [SyncVar] public int playerIndex;

        
        public NetworkMatch networkMatch;


        //public NetworkPlayer networkPlayer;        
        public GameObject uiPlayer;
        public Camera playerCamera;
        public AudioListener audioListener;


        internal virtual void Start()
        {
            
            networkMatch = GetComponent<NetworkMatch>();           
            networkMatch.gameObject.SetActive(true);

            playerCamera = GetComponentInChildren<Camera>();
            audioListener = GetComponentInChildren<AudioListener>();
            audioListener.enabled = false;
            playerCamera.enabled = false;
            

            Debug.Log(networkMatch);
        }

        public void SetUiPlayerOfNetworkPlayer(GameObject uiPlayer)
        {
            this.uiPlayer = uiPlayer;
        }

        #region HOST COMMANDS
        public void HostGame()
        {
            string _matchID = MatchMaker.GetRandomMatchID();
            peerIsTheHost = true;
            CmdHostGame(_matchID);
        }

        [Command]
        void CmdHostGame(string _matchID)
        {
            matchID = _matchID;
            networkMatch = GetComponent<NetworkMatch>();
            if (MatchMaker.istance.HostGame(_matchID, gameObject, out playerIndex))
            {
                Debug.Log($"<color = green>Game hosted succesfully</color>");
                networkMatch.matchId = _matchID.toGuid();
                TargetHostGame(true, _matchID, playerIndex);
            }
            else
            {
                Debug.Log("<color = red>Game hosted failed</color>");
                TargetHostGame(false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetHostGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchId: {matchID} == {_matchID}");
            UILobby.istance.HostSucces(success, _matchID, playerIndex);
        }

        #endregion

        #region JOIN MATCH
        public void JoinGame(string _inputID)
        {

            CmdJoinGame(_inputID);
        }

        [Command]
        void CmdJoinGame(string _matchID)
        {
            matchID = _matchID;

            if (MatchMaker.istance.JoinGame(_matchID, gameObject, out playerIndex))
            {
                Debug.Log($"<color = green>Game joined succesfully</color>" + playerIndex);
                networkMatch.matchId = _matchID.toGuid();
                Debug.Log(networkMatch.matchId);
                TargetJoinGame(true, _matchID, playerIndex);
            }
            else
            {
                Debug.Log("<color = red>Game join failed</color>");
                TargetJoinGame(false, _matchID, playerIndex);
            }
        }

        [TargetRpc]
        void TargetJoinGame(bool success, string _matchID, int _playerIndex)
        {
            playerIndex = _playerIndex;
            matchID = _matchID;
            Debug.Log($"MatchId: {matchID} == {_matchID}");
            UILobby.istance.JoinSucces(success, _matchID, playerIndex);
        }

        #endregion

        #region BEGIN GAME

        public void BeginGame()
        {

            CmdBeginGame();
        }

        [Command]
        void CmdBeginGame()
        {

            MatchMaker.istance.BeginGame(matchID);
            //Debug.Log("<color = red>Game Beginning</color>");


        }


        public void StartGame(List<GameObject> players)
        {
            TargetBeginGame(players);
        }

        [TargetRpc]
        void TargetBeginGame(List<GameObject> players)
        {
            //Debug.Log($"MatchId: {matchID} || beginning");


            // caricare altre scene
            DontDestroyOnLoad(this.gameObject);
            foreach (var player in players)
            {
                DontDestroyOnLoad(player);                
            }
            if(isClient)
                FindObjectOfType<UILobby>().gameObject.SetActive(false);
            Camera camera = FindObjectOfType<Camera>();
            if (camera.tag == "External Camera")
                camera.gameObject.SetActive(false);
            SceneManager.LoadSceneAsync(3, LoadSceneMode.Additive);


            
            NetworkServer.SpawnObjects();
            //if (isLocalPlayer)
            //{
            //    //RenderPlayerBody();
            //}
        }
        #endregion

       

    }

    
}
