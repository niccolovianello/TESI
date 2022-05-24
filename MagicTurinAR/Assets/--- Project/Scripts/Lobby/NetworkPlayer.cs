using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MirrorBasics;
using Mirror;
using UnityEngine.SceneManagement;
using Mapbox.Unity.Utilities;
using UnityEngine.EventSystems;
using Mapbox.Unity.Map;
using Mapbox.Utils;

namespace MirrorBasics
{


    public class NetworkPlayer : LobbyNetworkPlayer
    {
        [SyncVar(hook = nameof(SetTypeRole))]
        public int TypePlayerIndex = 3;

        public TypePlayer TypePlayerEnum = TypePlayer.Default;


       
        
        public static NetworkPlayer localPlayer;
        private UILobby UILobby;
        public GameObject playerBody;


        public enum TypePlayer
        {
            Explorer,
            Wiseman,
            Hunter,
            Default
            
        }

        internal override void Start()
        {
            base.Start();

            UILobby = FindObjectOfType<UILobby>();
            UILobby.ClientOrServerView(this);
            //SetRole(TypePlayer.Default);
            

            //Debug.Log("START PLAYER BEHAVIOUR");

           


            // loading da overridta from Firebase realtime database

           
                


            if (isLocalPlayer)
            {
                //Debug.Log("Is local player");
                localPlayer = this;
                

            }
            else
            {

                //Debug.Log("Is not local player");
                UILobby.istance.SpawnPlayerUIPrefab(this);


            }

        }
        private void ReceiveMagicOrGemsVibration()
        {
            Vibration.Vibrate();
        }

        private void OnDestroy()
        {
            //Debug.Log("DESTROY PLAYER BEHAVIOUR");

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
                //if (uiPlayer.networkPlayer.isLocalPlayer)
                //    uiPlayer.SetUsername(username);
            }
        }

        [Command]

        void ClientEnterInLobby()
        {
            RpcClientEnteredInLobby();
        }
        
        [ClientRpc]

        void RpcClientEnteredInLobby()
        {
            foreach (UIPlayer np in FindObjectsOfType<UIPlayer>())
            {
                np.SetPlayer(np.networkPlayer);

            }
        }

      

        [Command]
        public void CmdSendWhiteMagic(GameObject target, float whiteMagicToSend)
        {
            //Debug.Log("Wiseman manda magia bianca");
            NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
            
            RpcReceiveWhiteMagic( whiteMagicToSend, this.netId,opponentIdentity.netId);
        }


        [ClientRpc]
        private void RpcReceiveWhiteMagic( float whiteMagicReceived, uint netID1, uint netID2)
        {
           

           
            NetworkPlayer netplay1 = null;
            NetworkPlayer netplay2 = null;
            foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
            {
                if (np.netId == netID1)
                    netplay1 = np;
                if (np.netId == netID2)
                    netplay2 = np;
              
            }


            MagicPlayer magicPlayer = netplay2.GetComponent<MagicPlayer>();
           

            if (magicPlayer is Explorer)
            {
                Explorer explorer = magicPlayer.GetComponent<Explorer>();
                Debug.Log(explorer);
                explorer.IncreaseMana(whiteMagicReceived);
                ReceiveMagicOrGemsVibration();
            }
            if (magicPlayer is Hunter)
            {
                Hunter hunter = magicPlayer.GetComponent<Hunter>();
                Debug.Log(hunter);
                hunter.IncreaseMana(whiteMagicReceived);
                ReceiveMagicOrGemsVibration();
            }

            if (netplay1 != null && netplay2 != null)
                FindObjectOfType<GraphicInterctionBetweenPlayers>().WisemanSendWhiteMagic(netplay1.gameObject, netplay2.gameObject);
            else
                Debug.LogError("There's a problem with the network Ids");

         
        }

    



        [Command]
        public void CmdSendGem()
        {

            RpcReceiveGem(this.netId);
        }


        [ClientRpc]
        private void RpcReceiveGem(uint netID)
        {
            MagicPlayer magicPlayer = FindObjectOfType<MagicPlayer>();
            NetworkPlayer netplay1 = null;
            NetworkPlayer netplay2 = null;
            foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
            {
                if (np.netId == netID)
                    netplay1 = np;
                if (np.TypePlayerEnum == TypePlayer.Wiseman)
                    netplay2 = np;
            }

            if (magicPlayer is Wiseman)
            {
                
                Wiseman wiseman = magicPlayer.GetComponent<Wiseman>();
                wiseman.IncrementGems();
                ReceiveMagicOrGemsVibration();
            }

            if (netplay1 != null && netplay2 != null)
            {
                FindObjectOfType<GraphicInterctionBetweenPlayers>().ExplorerSendGem(netplay1.gameObject, netplay2.gameObject);
            }
            
            
        }

        [Command]
        public void CmdBeginNextMission()
        {

            RcpBeginNextMission();
        }


        [ClientRpc]
        private void RcpBeginNextMission()
        {
            MissionsManager MM = FindObjectOfType<MissionsManager>();
            MM.ChangeLevel();
           
        }

        public void RenderPlayerBody()
        {
            playerBody.SetActive(true);
        }

        public void NotRenderPlayerBody()
        {
            playerBody.SetActive(false);
        }

        [Command]
        public void CmdDestroyGem( )
        {

            RpcDestroyGem();
        }


        [ClientRpc]
        private void RpcDestroyGem()
        {

            MagicPlayer mp = FindObjectOfType<MagicPlayer>();

            if (mp is Hunter)
            {


                FindObjectOfType<UIManager>().OpenWindowToDestroyGemHunter();
                //GameManager gm = FindObjectOfType<GameManager>();
                //SceneManager.LoadSceneAsync(sceneToDestroyGem, LoadSceneMode.Additive);
                //gm.DisableMainGame();
                //NotRenderPlayerBody();
                //gm.PlayerCameraObject.SetActive(false);
            }

           
        }

        [Command]
        public void CmdSendWhiteMagicFromGem(int nfragment)
        {
            RpcWhiteMagicFromGem(nfragment);
        }


        [ClientRpc]
        private void RpcWhiteMagicFromGem(int nfragment)
        {

            MagicPlayer mp = FindObjectOfType<MagicPlayer>();

            if (mp is Wiseman)
            {
                foreach (MagicItemSO item in ItemAssets.Instance.magicInventorySO.items)
                {
                    if (item.id == 2000) // WhiteFragment specific code
                    {
                        item.prefab.GetComponent<MagicItem>().amount+=nfragment;
                        FindObjectOfType<UIInventory>().UpdateWhiteFragmentCount(item.prefab.GetComponent<MagicItem>().amount);

                        ReceiveMagicOrGemsVibration();

                    }


                }
            }
        }

        [Command]
        public void CmdSendGeoPositionToServer(float latitude, float longitude, uint netId)
        {
            RpcReceiveGeoPositionFromServer(latitude, longitude, netId);
        }

        [ClientRpc]
        private void RpcReceiveGeoPositionFromServer(float latitude, float longitude, uint netId)
        {
            //Debug.Log("Rpc Receive "+ latitude + "  "+ longitude + "  "+ netId);
            foreach(NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
            {
                if (np.netId == netId)
                {
                    AbstractMap _map = FindObjectOfType<AbstractMap>();
                    if (_map == null)
                        return;
                    np.gameObject.transform.MoveToGeocoordinate(latitude,longitude, _map.CenterMercator, _map.WorldRelativeScale);
                }


            }
        }
        [Command]
        public void CmdSharedMission()
        {
            RpcBeginSharedMission();
        }

        [ClientRpc]

        private void RpcBeginSharedMission()
        {
            MissionsManager misman = FindObjectOfType<MissionsManager>();
            misman.BeginSharedMission();
            misman.CloseStartMissionWindow();
            misman.btnStartWindow.gameObject.SetActive(true);
        }



        [Command]
        public void CmdAddMagicItem(string magicItemName)
        {
            RpcAddMagicItem(magicItemName);
        }

        [ClientRpc]

        private void RpcAddMagicItem(string magicItemName)
        {
            TurnManager turnManager = FindObjectOfType<TurnManager>();
            foreach (MagicItemSO miSO in ItemAssets.Instance.magicInventorySO.items)
            {
                if(miSO.name == magicItemName)
                    turnManager.AddItemPicked(miSO);
            }
                
        }


        [Command]
        public void CmdRotateLockWheel(string wheelName)
        {
            RpcRotateLockWheel(wheelName);
        }

        [ClientRpc]

        private void RpcRotateLockWheel(string wheelName)
        {

            GameObject.Find(wheelName).GetComponent<Rotate>().RotateWheel();
        }

        [Command]
        public void CmdGoToStats()
        {
            RpcGoToStats();
        }

        [ClientRpc]

        private void RpcGoToStats()
        {

            FindObjectOfType<MissionsManager>().GoToStatsOfTheMatch();
        }

    }



}
