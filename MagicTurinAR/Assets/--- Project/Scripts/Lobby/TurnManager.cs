using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace MirrorBasics
{
    public class TurnManager : NetworkBehaviour
    {
        public List<LobbyNetworkPlayer> players = new List<LobbyNetworkPlayer>();

        private List<MagicItemSO> itemsPicked = new List<MagicItemSO>();

        private string matchData = "0/0/0";


        public List<MagicItemSO> ItemsPicked
        {
            get => itemsPicked;
        
        }


        

        public string MatchData
        {
            get => matchData;
            set => matchData = value;
        }
        public void AddPlayer(LobbyNetworkPlayer _networkPlayer)
        {
            players.Add(_networkPlayer);
        }

        public void AddItemPicked(MagicItemSO magicItemSO)
        {
            itemsPicked.Add(magicItemSO);
        }

       
    }
}
