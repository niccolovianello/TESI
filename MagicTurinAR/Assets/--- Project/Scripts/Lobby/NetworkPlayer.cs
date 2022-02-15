using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using Mirror;
using UnityEngine.SceneManagement;
    

namespace MirrorBasics
{


    public class NetworkPlayer : LobbyNetworkPlayer
    {
        [SyncVar(hook = nameof(SetTypeRole))]
        public int TypePlayerIndex;

        public TypePlayer TypePlayerEnum;


        public StoreData storeData;
        FirebaseManager firebaseManager;
        public static NetworkPlayer localPlayer;
        private UILobby UILobby;



        public enum TypePlayer
        {
            Explorer,
            Wiseman,
            Hunter
        }

        internal override void Start()
        {
            base.Start();

            UILobby = FindObjectOfType<UILobby>();
            UILobby.ClientOrServerView(this);


            Debug.Log("START PLAYER BEHAVIOUR");

            storeData = GetComponent<StoreData>();


            // loading da overridta from Firebase realtime database

            if (!isServer || (isServer && isClient))
            {
                firebaseManager = FindObjectOfType<FirebaseManager>();
                firebaseManager.StartCoroutine(firebaseManager.LoadUserData(storeData));
            }
                


            if (isLocalPlayer)
            {
                Debug.Log("Is local player");
                localPlayer = this;

            }
            else
            {

                Debug.Log("Is not local player");
                UILobby.istance.SpawnPlayerUIPrefab(this);

            }
           
        }

        private void OnDestroy()
        {
            Debug.Log("DESTROY PLAYER BEHAVIOUR");

        }

        [Command]

        public void SetRole(TypePlayer roleIndex)
        {
            
            TypePlayerEnum = roleIndex;
            TypePlayerIndex = (int)roleIndex;
            storeData.SetRoleData((int) roleIndex);
        }

        void SetTypeRole(int oldIndex, int newIndex)
        {
            TypePlayerIndex = newIndex;
            TypePlayerEnum = (TypePlayer)newIndex;

            UIPlayer[] playersUiPrefabs = FindObjectsOfType<UIPlayer>();
            foreach (UIPlayer uiPlayer in playersUiPrefabs)
            {
                uiPlayer.SetTextRole(uiPlayer.GetNetworkPlayer());
            }
        }

        [Command]
        public void CmdSendWhiteMagic(GameObject target, int whiteMagicToSend)
        {
            Debug.Log("Wiseman manda magia bianca");
            NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
            RpcTargetReceiveWhiteMagic(opponentIdentity.connectionToClient, whiteMagicToSend);
        }


        [TargetRpc]
        public void RpcTargetReceiveWhiteMagic(NetworkConnection target, int whiteMagicReceived)
        {
            MagicPlayer magicPlayer = FindObjectOfType<MagicPlayer>();
            
            if (magicPlayer is Explorer)
            {
                Explorer explorer = magicPlayer.GetComponent<Explorer>();
                Debug.Log(explorer);
                explorer.IncrementWhiteEnergy(whiteMagicReceived);
            }
            if (magicPlayer is Hunter)
            {
                Hunter hunter = magicPlayer.GetComponent<Hunter>();
                Debug.Log(hunter);
                hunter.IncrementWhiteEnergy(whiteMagicReceived);
            }
        }


        [Command]
        public void CmdSendGem()
        {

            Debug.Log("Send Gem");
            RpcReceiveGem();
        }


        [ClientRpc]
        public void RpcReceiveGem()
        {
            MagicPlayer magicPlayer = FindObjectOfType<MagicPlayer>();

            if (magicPlayer is Wiseman)
            {
                Wiseman wiseman = magicPlayer.GetComponent<Wiseman>();
                Debug.Log("Wiseman riceve gemma");
                wiseman.IncrementGems();
            }
            
            
        }

        [Command]
        public void CmdBeginNextMission()
        {

            RcpBeginNextMission();
        }


        [ClientRpc]
        public void RcpBeginNextMission()
        {
            MissionsManager MM = FindObjectOfType<MissionsManager>();
            MM.ChangeLevel();
            MM.StartMission();
        }
    }
}
