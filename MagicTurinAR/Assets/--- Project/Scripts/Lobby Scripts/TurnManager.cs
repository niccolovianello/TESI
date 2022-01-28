using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace MirrorBasics
{
    public class TurnManager : NetworkBehaviour
    {
        public List<LobbyNetworkPlayer> players = new List<LobbyNetworkPlayer>();
        public void AddPlayer(LobbyNetworkPlayer _networkPlayer)
        {
            players.Add(_networkPlayer);
        }
    }
}
