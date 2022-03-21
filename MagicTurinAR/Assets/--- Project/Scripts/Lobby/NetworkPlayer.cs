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
        public int TypePlayerIndex;

        public TypePlayer TypePlayerEnum;


       
        
        public static NetworkPlayer localPlayer;
        private UILobby UILobby;
        public GameObject playerBody;



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

           


            // loading da overridta from Firebase realtime database

           
                


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
        private void ReceiveMagicOrGemsVibration()
        {
            Vibration.Vibrate();
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
        public void CmdSendWhiteMagic(GameObject target, float whiteMagicToSend)
        {
            Debug.Log("Wiseman manda magia bianca");
            NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
            
            RpcTargetReceiveWhiteMagic(opponentIdentity.connectionToClient, whiteMagicToSend, this.netId,opponentIdentity.netId);
        }


        [ClientRpc]
        public void RpcTargetReceiveWhiteMagic(NetworkConnection target, float whiteMagicReceived, uint netID1, uint netID2)
        {
            Debug.Log("rpc");

           
            NetworkPlayer netplay1 = null;
            NetworkPlayer netplay2 = null;
            foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
            {
                if (np.netId == netID1)
                    netplay1 = np;
                if (np.netId == netID2)
                    netplay2 = np;
              
            }
            Debug.Log("RpcTargetReceiveWhiteMagic: player1" + netplay1 + "    player2" + netplay2);

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

            Debug.Log("Send Gem");
            RpcReceiveGem(this.netId);
        }


        [ClientRpc]
        public void RpcReceiveGem(uint netID)
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
                Debug.Log("Wiseman riceve gemma");
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
        public void RcpBeginNextMission()
        {
            MissionsManager MM = FindObjectOfType<MissionsManager>();
            MM.ChangeLevel();
            MM.StartMission();
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
            Debug.Log("CMD mandare gemma");
        }


        [ClientRpc]
        public void RpcDestroyGem()
        {
            Debug.Log("Gemma arrivata");
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
            Debug.Log("Gemma distrutta");
        }


        [ClientRpc]
        public void RpcWhiteMagicFromGem(int nfragment)
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
        public void CmdSendGeoPositionToServer(float latitude, float longitude,uint netId)
        {

            RpcReceiveGeoPositionFromServer(latitude, longitude, netId);
        
        }

        [ClientRpc]
        public void RpcReceiveGeoPositionFromServer(float latitude, float longitude, uint netId)
        {
            foreach(NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
            {
                if (np.netId == netId)
                {
                    AbstractMap am = FindObjectOfType<AbstractMap>();
                    Vector2d worldPosition2d= Conversions.GeoToWorldPosition(latitude, longitude, am.CenterMercator);
                    np.gameObject.transform.Translate(new Vector3((float)worldPosition2d.x,transform.position.y, (float)worldPosition2d.y), Space.World);
                }


            }
        }

    }


}
