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
         
            Explorer,
            Wiseman,
            Hunter,
            None
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
                case StoreDataScript.TextRole.Explorer:
                    role.text = "E";
                    break;
                case StoreDataScript.TextRole.Wiseman:
                    role.text = "W";
                    break;
                case StoreDataScript.TextRole.Hunter:
                    role.text = "H";
                    break;
                default:
                    role.text = "N";
                    break;
            }
        }

        public void SetTextRole(NetworkPlayer _player)
        {
            this.networkPlayer = _player;
            switch (_player.TypePlayerEnum)
            {
                case NetworkPlayer.TypePlayer.Explorer:
                    role.text = "E";
                    break;
                case NetworkPlayer.TypePlayer.Wiseman:
                    role.text = "W";
                    break;
                case NetworkPlayer.TypePlayer.Hunter:
                    role.text = "H";
                    break;
                default:
                    role.text = "N";
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
