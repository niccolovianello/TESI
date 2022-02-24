using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using Mirror;
using System.Text;
using System;

namespace MirrorBasics
{

    [System.Serializable]
    public class Match {

        public string matchID;
        readonly SyncListGameObject players = new SyncListGameObject(); 

        public Match(string matchID, GameObject player)
        {
            this.matchID = matchID;
            players.Add(player);


        }

        public SyncListGameObject GetPlayers()
        {
            return players;
        }

        public Match() { }
    }

    [System.Serializable]
    public class SyncListGameObject : SyncList<GameObject> { }

    [System.Serializable]
    public class SyncListMatch : SyncList<Match> { }

    [System.Serializable]
    public class SyncListString : SyncList<string> { }



    public class MatchMaker : NetworkBehaviour
    {
        public static MatchMaker istance;

        [SerializeField] public SyncListMatch matches = new SyncListMatch();

        [SerializeField] public SyncListString matchIDs = new SyncListString();

        [SerializeField] GameObject turnManagerPrefab;



        void Start()
        {
            istance = this;
        }


        public static string GetRandomMatchID()
        {
            string _ID = string.Empty;

            for (int i = 0; i < 5; i++)
            {
                int random = UnityEngine.Random.Range(0, 36);

                if (random < 26)
                {
                    _ID += (char)(random + 65);
                }
                else {
                    _ID += (random - 26).ToString();
                }
            }

            Debug.Log($"Random match id is: {_ID}");
            return _ID;
        }

        public bool HostGame(string _matchID, GameObject _player, out int playerIndex)
        {
            playerIndex = -1;

            if (!matchIDs.Contains(_matchID))
            {
                matchIDs.Add(_matchID);
                matches.Add(new Match(_matchID, _player));
                Debug.Log(" MatchId generated! ");
                playerIndex = 1;
                return true;
            }
            else
            {
                Debug.Log(" MatchId already exists! ");
                return false;
            }


        }

        public bool JoinGame(string _matchID, GameObject _player, out int playerIndex)
        {
            playerIndex = -1;

            if (matchIDs.Contains(_matchID))
            {
                for (int i = 0; i < matches.Count; i++)
                {
                    if (matches[i].matchID == _matchID)
                    {
                        matches[i].GetPlayers().Add(_player);
                        playerIndex = matches[i].GetPlayers().Count ;
                        Debug.Log(playerIndex + " number of players");
                        break;
                    }
                
                }
                
                Debug.Log(" MatchId joined! ");
                return true;
            }
            else
            {
                Debug.Log(" MatchId does not exist! ");
                return false;
            }


        }
        public void BeginGame(string _matchID)
        {
            GameObject newTurnManager = Instantiate(turnManagerPrefab);
            NetworkServer.Spawn(newTurnManager);
            newTurnManager.GetComponent<NetworkMatch>().matchId = _matchID.toGuid();

            //salvataggio partita e elenco giocatori

            TurnManager turnManager = newTurnManager.GetComponent<TurnManager>();

            DontDestroyOnLoad(newTurnManager);


            for (int i = 0; i < matches.Count; i++)
            {
                if (matches[i].matchID == _matchID)
                {
                    List<GameObject> playersList = new List<GameObject>();
                    foreach (var player in matches[i].GetPlayers())
                    {
                        playersList.Add(player);
                    }

                    foreach (var player in matches[i].GetPlayers())
                    {
                        LobbyNetworkPlayer _player = player.GetComponent<LobbyNetworkPlayer>();
                        turnManager.AddPlayer(_player);

                        if (matches[i].GetPlayers().Count >= 1)
                            _player.StartGame(playersList);
                        else
                            Debug.LogError("There are not enough players or someone does not choose his role yet!");
                        //if (_player.isLocalPlayer) ;
                        //    _player.RenderPlayerBody();
                    }

                   
                    break;
                }
            }

        }

    }


    public static class MatchExtensions
    {
        public static Guid toGuid(this string id)
        {
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(id);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            return new Guid(hashBytes);
        }
    }
    

}




