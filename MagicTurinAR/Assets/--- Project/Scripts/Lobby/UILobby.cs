using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MirrorBasics
{
    public class UILobby : MonoBehaviour
    {

        public static UILobby istance;
        public Canvas totalLobbyUI;
        public StoryTellingUI storyTelling;


        [Header("LoadGameCanvas")]
        [SerializeField] Canvas loadGameCanvas;

        [Header("Host Join")]
        [SerializeField] GameObject hostJoinCanvas;
        [SerializeField] InputField joinMatchInput;
        [SerializeField] Button joinButton;
        [SerializeField] Button hostButton;
        [SerializeField] Canvas lobbyCanvas;

        [Header("Lobby")]
        [SerializeField] Transform UIPlayerParent;
        [SerializeField] GameObject UIPlayerPrefab;
        [SerializeField] Text matchIDText;
        [SerializeField] GameObject beginGameButton;

        [Header("DescriptionBox")]
        [SerializeField] Canvas boxDescription;

        [Header("ServerUI")]
        [SerializeField] Canvas ServerUI;
        [SerializeField] TMP_Text textNumberClients;
        private int numberClients = 0;

        [Header("ClientUI")]

        [SerializeField] public Canvas ClientUI;

        public int GetNumberClients()
        {
            return numberClients;
        }

        public void SetNumberClients(int n)
        {
            numberClients = n;
            textNumberClients.text = numberClients.ToString();
        }



        public enum TypePlayer
        {
            Explorer,
            Wiseman,
            Hunter
        }

        

        void Start()
        {
            istance = this;
        }
        public void Host()
        {
            joinButton.interactable = false;
            joinMatchInput.interactable = false;
            hostButton.interactable = false;

            NetworkPlayer.localPlayer.HostGame();
           

        }

        public void HostSucces(bool success, string _matchID, int playerIndex)
        {
            if (success) {
                lobbyCanvas.enabled = true;

                matchIDText.text = _matchID;
                Debug.Log("Spawn Host!");
                if(playerIndex != -1 )
                    SpawnPlayerUIPrefab(NetworkPlayer.localPlayer);               
                beginGameButton.SetActive(true);

                //Debug.Log(lobbyCanvas.enabled);
            }
            else {
                joinButton.interactable = true;
                joinMatchInput.interactable = true;
                hostButton.interactable = true;
            }
        }

        public void Join()
        {

            joinButton.interactable = false;
            joinMatchInput.interactable = false;
            hostButton.interactable = false;

            NetworkPlayer.localPlayer.JoinGame(joinMatchInput.text.ToUpper());
        }

        public void JoinSucces(bool success, string _matchID, int playerIndex)
        {
            if (success && playerIndex>1)
            {
                lobbyCanvas.enabled = true;
                //Debug.Log("Spawn Client!");
                SpawnPlayerUIPrefab(NetworkPlayer.localPlayer);
                matchIDText.text = _matchID;
               
            }
            else
            {
                //Debug.Log("Not spwan client");
                joinButton.interactable = true;
                joinMatchInput.interactable = true;
                hostButton.interactable = true;
            }
        }

        public void SpawnPlayerUIPrefab(NetworkPlayer networkPlayer)
        {
            GameObject newUIPlayer = Instantiate(UIPlayerPrefab, UIPlayerParent);
            newUIPlayer.GetComponent<UIPlayer>().SetPlayer(networkPlayer);
            
            networkPlayer.SetUiPlayerOfNetworkPlayer(newUIPlayer);
           
            newUIPlayer.transform.SetSiblingIndex(networkPlayer.playerIndex - 1);
        }

        public void OpenPlayerDescription()
        {
            boxDescription.enabled = true;
            beginGameButton.SetActive(false);
        }

        public void ApplyExplorerRole()
        {
            NetworkPlayer.localPlayer.SetRole(NetworkPlayer.TypePlayer.Explorer);
            boxDescription.enabled = false;
            if(NetworkPlayer.localPlayer.peerIsTheHost ==true)
                beginGameButton.SetActive(true);

            NetworkPlayer.localPlayer.uiPlayer.GetComponent<UIPlayer>().SetTextRole(NetworkPlayer.localPlayer);
          
        }

        public void ApplyHunterRole()
        {
            NetworkPlayer.localPlayer.SetRole(NetworkPlayer.TypePlayer.Hunter);
            boxDescription.enabled = false;
            if (NetworkPlayer.localPlayer.peerIsTheHost == true)
                beginGameButton.SetActive(true);
            NetworkPlayer.localPlayer.uiPlayer.GetComponent<UIPlayer>().SetTextRole(NetworkPlayer.localPlayer);
        }

        public void ApplyWiseManRole()
        {
            NetworkPlayer.localPlayer.SetRole(NetworkPlayer.TypePlayer.Wiseman);
            boxDescription.enabled = false;
            if (NetworkPlayer.localPlayer.peerIsTheHost == true)
                beginGameButton.SetActive(true);
            NetworkPlayer.localPlayer.uiPlayer.GetComponent<UIPlayer>().SetTextRole(NetworkPlayer.localPlayer);
        }

        public void ClientOrServerView(NetworkPlayer np)
        {
           

            if (np.isClient)
           
            {
                
                ServerUI.GetComponent<Canvas>().enabled = false;
                ClientUI.GetComponent<Canvas>().enabled = true;
            }



        }

        public void BeginStoryTelling()
        {
            lobbyCanvas.enabled = false;
            totalLobbyUI.enabled = false;
            NetworkPlayer.localPlayer.BeginStoryTelling();
        }


        public void BeginGame()
        {
            lobbyCanvas.enabled = false;
            totalLobbyUI.enabled = false;
            NetworkPlayer.localPlayer.BeginGame();
        }

        public void NewGame()
        {
            hostJoinCanvas.SetActive(true);
        }

        public void LoadGame()
        {
            loadGameCanvas.enabled = true;
        }

       

    }
}
