using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace MirrorBasics
{
    public class UIPlayer : MonoBehaviour
    {

        [Range(0.5f, 3.0f)]
        [SerializeField] float controlOnNetworkPlayerTime = 1.5f;
        [SerializeField] Text text;
        [SerializeField] Text roleText;
        [SerializeField] Image roleImage;
        [SerializeField] Sprite explorerSprite;
        [SerializeField] Sprite hunterSprite;
        [SerializeField] Sprite wisemanSprite;

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
            StartCoroutine(ControlOnNetworkPlayer());
        }

        public NetworkPlayer GetNetworkPlayer()
        {
            return networkPlayer;        
        }
        public void SetPlayer(NetworkPlayer _player)
        {
            this.networkPlayer = _player;
            text.text = networkPlayer.username;
            switch (_player.storeData.role)
            {
                case StoreData.TextRole.Explorer:
                    roleText.text = "Explorer";
                    roleImage.sprite = explorerSprite;
                    break;
                case StoreData.TextRole.Wiseman:
                    roleText.text = "Wiseman";
                    roleImage.sprite = wisemanSprite;
                    break;
                case StoreData.TextRole.Hunter:
                    roleText.text = "Hunter";
                    roleImage.sprite = hunterSprite;
                    break;
                default:
                    roleText.text = "N";
                    break;
            }
        }

        public void SetTextRole(NetworkPlayer _player)
        {
            this.networkPlayer = _player;
            text.text = networkPlayer.username;
            switch (_player.TypePlayerEnum)
            {
                case NetworkPlayer.TypePlayer.Explorer:
                    roleText.text = "Explorer";
                    roleImage.sprite = explorerSprite;
                    break;
                case NetworkPlayer.TypePlayer.Wiseman:
                    roleText.text = "Wiseman";
                    roleImage.sprite = wisemanSprite;
                    break;
                case NetworkPlayer.TypePlayer.Hunter:
                    roleText.text = "Hunter";
                    roleImage.sprite = hunterSprite;
                    break;
                default:
                    roleText.text = "N";
                    break;
            }
        }

        public void OpenRolesDescriptions()
        {
            if (!networkPlayer.isLocalPlayer)
                return;
            uiLobby.OpenPlayerDescription();         
        }
        private IEnumerator ControlOnNetworkPlayer()
        {
            while (true)
            {
                yield return new WaitForSeconds(controlOnNetworkPlayerTime);

                if (networkPlayer == null)
                {
                    Destroy(this.gameObject);
                    break;
                }
            }
        }
    }
}
