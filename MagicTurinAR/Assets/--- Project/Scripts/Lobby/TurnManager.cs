using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mirror;


namespace MirrorBasics
{
    public class TurnManager : NetworkBehaviour
    {
        private Guid matchGUID;

        public List<LobbyNetworkPlayer> players = new List<LobbyNetworkPlayer>();

        private List<MagicItemSO> itemsPicked = new List<MagicItemSO>();

        private long matchDataMS = 0;

        private string matchDataString = "0/0/00";

        private string jsonLoader;

        void Start()
        {
            matchGUID = GetComponent<NetworkMatch>().matchId;
        }
        public List<MagicItemSO> ItemsPicked
        {
            get => itemsPicked;
        
        }


        public Guid MatchGUID
        {
            get => matchGUID;
            set => matchGUID = value;
        }

        public string JsonLoader
        {
            get => jsonLoader;
            set => jsonLoader = value;
        }
        public long MatchDataMS
        {
            get => matchDataMS;
            set => matchDataMS = value;
        }

        public string MatchDataString
        {
            get => matchDataString;
            set => matchDataString = value;
        }
        public void AddPlayer(LobbyNetworkPlayer _networkPlayer)
        {
            players.Add(_networkPlayer);
        }

        public void AddItemPicked(MagicItemSO magicItemSO)
        {
            itemsPicked.Add(magicItemSO);

            
        }

        public JsonHelper.MyMatch CreateMatchJson()
        {
            JsonHelper.MyMatch match = new JsonHelper.MyMatch();

            match.dataMS = DateTime.UtcNow.Millisecond;
            match.dataString = DateTime.UtcNow.ToString();

            List<JsonHelper.MatchPlayers> playerInstance = new List<JsonHelper.MatchPlayers>();
           

            foreach(NetworkPlayer np in players)
            {
                JsonHelper.MatchPlayers player = new JsonHelper.MatchPlayers();
                player.username = np.username;
                JsonHelper.MatchPlayerCollectable collectables = new JsonHelper.MatchPlayerCollectable();

                foreach (MagicItemSO miSO in itemsPicked)
                {
                    switch (np.TypePlayerEnum)
                    {
                        case NetworkPlayer.TypePlayer.Explorer:
                            if (miSO.itemType == MagicItemSO.ItemType.Artifact)
                            {                                
                                collectables.collectableName.Add(miSO.name);                                
                            }
                                
                            break;
                        case NetworkPlayer.TypePlayer.Wiseman:
                            if (miSO.itemType == MagicItemSO.ItemType.Book)
                            {
                                collectables.collectableName.Add(miSO.name);
                            }

                            break;
                        case NetworkPlayer.TypePlayer.Hunter:
                            if (miSO.itemType == MagicItemSO.ItemType.Rune)
                            {
                                collectables.collectableName.Add(miSO.name);
                            }
                            break;
                    }
                }

                player.collectables.Add(collectables);
                playerInstance.Add(player);
            }
            match.players = playerInstance;

            

            return match;
        }

        public string GetPreviousMatch()
        {
            FindObjectOfType<FirebaseManager>().LoadMatchData(this);
            if (jsonLoader == null)
                return null;
            else
                return JsonLoader;
        }



    }

    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Matches;
        }

        public static string ToJson<T>(T[] array)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Matches = array;
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            Wrapper<T> wrapper = new Wrapper<T>();
            wrapper.Matches = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);

        }
        [Serializable]
        private class Wrapper<T>
        {
            public T[] Matches;
        }

        public static JsonHelper.MyMatch[] ParseJsonToObject<MyMatch>(string json)
        {
            var wrappedjsonArray = JsonUtility.FromJson<MyWrapper>(json);
            return wrappedjsonArray.matches;
        }

        public static string ParseObjectToJson<MyMatch>(JsonHelper.MyMatch[] array,string json)
        {
            MyWrapper wrapper = new MyWrapper();
            wrapper.matches = array;
            return JsonUtility.ToJson(wrapper, true);
            
        }



        [Serializable]
        public class MyWrapper
        {
            public MyMatch[] matches;
        }

        [Serializable]
        public class MyMatch
        {
            public long dataMS;
            public string dataString;
            public List<MatchPlayers> players;
        }
        [Serializable]
        public class MatchPlayers
        {
            public string username;
            public List<MatchPlayerCollectable> collectables;
        }

        [Serializable]
        public class MatchPlayerCollectable
        {
            public List<string> collectableName;
        }
    }




}
