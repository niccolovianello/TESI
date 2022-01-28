using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace MirrorBasics
{
    public class AutoHostClient : MonoBehaviour
    {
        [SerializeField] NetworkManager networkManager;

        void Start()
        {
            if (!Application.isBatchMode)
            {
                //Headless build
                Debug.Log("*** Client build ***");
                networkManager.StartClient();
            }
            else
            {
                Debug.Log("*** Server build ***");
                networkManager.StartServer();
            }
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                networkManager.StartServer();
                
            }
        }

        public void JoinLocal()
        {
            networkManager.networkAddress = "arlocationbased.ddns.net";
            networkManager.StartClient();

            //arlocationbased.ddns.net
        }

    }
}
