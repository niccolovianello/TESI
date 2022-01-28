using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MirrorBasics
{
    public class UIPlayerScript : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] Text role;
        
        public enum TextRole
        {
        
            None = 0,
            Explorer = 1,
            Wiseman = 2,
            Hunter = 3
        

    }

        [SerializeField] UILobby uiLobby;
        public NetworkPlayer networkPlayer;
        
        private void Start()
        {
            uiLobby = FindObjectOfType<UILobby>();

           
        }

        public NetworkPlayer GetNetworkPlayer()
        {
            return networkPlayer;        
        }
        public void SetPlayer(NetworkPlayer _player)
        {
            this.networkPlayer = _player;
            text.text = "Player " + networkPlayer.playerIndex.ToString();
            switch (_player.storeDataScript.role)
            {
                case 0:
                    role.text = "N";
                    break;
                case 1:
                    role.text = "E";
                    break;
                case 2:
                    role.text = "W";
                    break;
                case 3:
                    role.text = "H";
                    break;
            }
        }

        public void SetTextRole(NetworkPlayer _player)
        {
            this.networkPlayer = _player;
            switch (_player.TypePlayerIndex)
            {
                case 0:
                    role.text = "N";
                    break;
                case 1:
                    role.text = "E";
                    break;
                case 2:
                    role.text = "W";
                    break;
                case 3:
                    role.text = "H";
                    break;
            }
        }

        public void OpenRolesDescriptions()
        {
            if (!networkPlayer.isLocalPlayer)
                return;
            uiLobby.OpenPlayerDescription();         
        }
    }
}
