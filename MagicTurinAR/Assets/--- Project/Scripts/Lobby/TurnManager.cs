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

        private FirebaseManager FBM;

        private bool loaderFlag = false;

        
        void Start()
        {
            FBM = FindObjectOfType<FirebaseManager>();
            matchGUID = GetComponent<NetworkMatch>().matchId;
        }
        public List<MagicItemSO> ItemsPicked
        {
            get => itemsPicked;
        
        }

        public bool LoaderFlag
        {
            get => loaderFlag;
            set => loaderFlag = value;
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

            //Debug.Log("CreateMatch");
            JsonHelper.MyMatch match = new JsonHelper.MyMatch();

            match.dataMS = DateTime.UtcNow.Millisecond;
            match.dataString = DateTime.UtcNow.ToString();

            List<JsonHelper.MatchPlayers> playerInstances = new List<JsonHelper.MatchPlayers>();
           

            foreach(NetworkPlayer np in players)
            {
                JsonHelper.MatchPlayers player = new JsonHelper.MatchPlayers();
                player.username = np.username;
                List<JsonHelper.MatchPlayerCollectable> collectables = new List<JsonHelper.MatchPlayerCollectable>();

                foreach (MagicItemSO miSO in itemsPicked)
                {
                    JsonHelper.MatchPlayerCollectable coll = new JsonHelper.MatchPlayerCollectable();
                    switch (np.TypePlayerEnum)
                    {
                        case NetworkPlayer.TypePlayer.Explorer:
                            if (miSO.itemType == MagicItemSO.ItemType.Artifact)
                            {                                
                                coll.collectableName = miSO.name;                                
                            }
                                
                            break;
                        case NetworkPlayer.TypePlayer.Wiseman:
                            if (miSO.itemType == MagicItemSO.ItemType.Book)
                            {
                                coll.collectableName = miSO.name;
                            }

                            break;
                        case NetworkPlayer.TypePlayer.Hunter:
                            if (miSO.itemType == MagicItemSO.ItemType.Rune)
                            {
                                coll.collectableName = miSO.name;
                            }
                            break;
                    }
                    collectables.Add(coll);
                }

                player.collectables = collectables;
                playerInstances.Add(player);
            }
            match.players = playerInstances;

            

            return match;
        }

        

       

        public IEnumerator LoadMatchOnFirebaseCoroutine()
        {
            //Debug.Log("LoadCoroutine");
            loaderFlag = false;
            jsonLoader = null;
            List<JsonHelper.MyMatch> matches = new List<JsonHelper.MyMatch>();

            StartCoroutine(FBM.LoadMatchData(this));

            yield return new WaitUntil(predicate: () => this.LoaderFlag);

            if (jsonLoader != null)
            {
                matches = JsonHelper.ParseJsonToObject<JsonHelper.MyMatch>(jsonLoader);
            }

            //Debug.Log("Wait until passato");
            matches.Add(CreateMatchJson());
            string result = JsonHelper.ParseObjectToJson<JsonHelper.MyMatch>(matches, true);
            StartCoroutine(FBM.UpdateMatches(result));
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

        public static List<JsonHelper.MyMatch> ParseJsonToObject<MyMatch>(string json)
        {
           
            var wrappedjsonArray = JsonUtility.FromJson<MyWrapper>(json);
            return wrappedjsonArray.matches;
        }

        public static string ParseObjectToJson<MyMatch>(List<JsonHelper.MyMatch> list,bool prettyPrint)
        {
            MyWrapper wrapper = new MyWrapper();
            wrapper.matches = list;
            return JsonUtility.ToJson(wrapper, prettyPrint);
            
        }



        [Serializable]
        public class MyWrapper
        {
            public List<MyMatch> matches;
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
            public string collectableName;
        }
    }




}
