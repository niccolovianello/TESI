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
        public void CmdSendWhiteMagic(GameObject target, float whiteMagicToSend)
        {
            Debug.Log("Wiseman manda magia bianca");
            NetworkIdentity opponentIdentity = target.GetComponent<NetworkIdentity>();
            RpcTargetReceiveWhiteMagic(opponentIdentity.connectionToClient, whiteMagicToSend);
        }


        [TargetRpc]
        public void RpcTargetReceiveWhiteMagic(NetworkConnection target, float whiteMagicReceived)
        {
            MagicPlayer magicPlayer = FindObjectOfType<MagicPlayer>();
            
            if (magicPlayer is Explorer)
            {
                Explorer explorer = magicPlayer.GetComponent<Explorer>();
                Debug.Log(explorer);
                explorer.IncreaseMana(whiteMagicReceived);
            }
            if (magicPlayer is Hunter)
            {
                Hunter hunter = magicPlayer.GetComponent<Hunter>();
                Debug.Log(hunter);
                hunter.IncreaseMana(whiteMagicReceived);
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
                    
                }


            }
        }

    }


}
