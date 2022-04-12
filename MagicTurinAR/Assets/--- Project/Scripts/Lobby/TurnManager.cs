using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


namespace MirrorBasics
{
    public class TurnManager : NetworkBehaviour
    {
        public List<LobbyNetworkPlayer> players = new List<LobbyNetworkPlayer>();

        private List<string> artifacts = new List<string>();
        private List<string> runes = new List<string>();
        private List<string> books = new List<string>();

        private string matchData = "0/0/0";


        public List<string> Artifacts
        {
            get => artifacts;
        
        }
        public List<string> Runes
        {
            get => runes;

        }
        public List<string> Books
        {
            get => books;

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

        public void AddArtifact(string artifactName)
        {
            artifacts.Add(artifactName);
        }

        public void AddRune(string runeName)
        {
            runes.Add(runeName);
        }

        public void AddBook(string bookName)
        {
            books.Add(bookName);
        }
    }
}
