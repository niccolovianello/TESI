using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using MirrorBasics;
using UnityEngine.SceneManagement;

public class NetworkManagerCustom : NetworkManager
{
    private UILobby uiLobby;
    public override void Start()
    {
        base.Start();
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

    }
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        base.OnServerAddPlayer(conn);

        Debug.Log("connesso");
        uiLobby = FindObjectOfType<UILobby>();
        uiLobby.SetNumberClients(uiLobby.GetNumberClients() + 1);
    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        base.OnServerDisconnect(conn);

        Debug.Log("disconnesso");
        uiLobby = FindObjectOfType<UILobby>();
        uiLobby.SetNumberClients(uiLobby.GetNumberClients() - 1);
    }

    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling)
    {
        base.OnClientChangeScene(newSceneName, sceneOperation, customHandling);

        
        //SceneManager.LoadSceneAsync("Explorer_Main", LoadSceneMode.Additive);
  }

}