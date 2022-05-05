using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class StoryTellingUI : MonoBehaviour
{
    public MagicStoryTellingSO storyTellingSO;
    public int currentFragmentIndex = 0 ;

    [Header("Frgment StoryTelling Components")]
    [SerializeField] private Image imageFragment;
    [SerializeField] private Text narratorFragment;
    [SerializeField] private Text titleFragment;
    [SerializeField] private Text textFragment;
    [SerializeField] private Text indexFragment;

    [SerializeField] private Button beginButton;

    private FragmentStoryTellingSO currentFragment;

    private void Start()
    {
        currentFragment = storyTellingSO.fragments[currentFragmentIndex];
        SetFragment();
       
        
    }

    public void NextFragment()
    {
        Debug.Log("Next");
        if (currentFragmentIndex < storyTellingSO.fragments.Count-1)
        {
            currentFragmentIndex++;
            currentFragment = storyTellingSO.fragments[currentFragmentIndex];
            SetFragment();

        }

        if (currentFragment.index == storyTellingSO.fragments.Count - 1 && NetworkPlayer.localPlayer.peerIsTheHost == true)
               
        {
            beginButton.gameObject.SetActive(true);
        }



    }

    public void BeginMainGame()
    {
        
        NetworkPlayer.localPlayer.BeginGame();
       
    }

    private void SetFragment()
    {
        string playerName = "";
        indexFragment.text = (currentFragmentIndex + 1) + " / " + storyTellingSO.fragments.Count;
        imageFragment.sprite = currentFragment.fragmentImage;
        narratorFragment.text = currentFragment.narratorText;
        titleFragment.text = currentFragment.fragmentTitle;

        switch (currentFragment.fragmentDestinataryType)
        {
            case (FragmentStoryTellingSO.PlayerType.Explorer):
                
                foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    if (np.TypePlayerEnum == NetworkPlayer.TypePlayer.Explorer)
                        playerName = np.username;

                textFragment.text = SetBetween(currentFragment.fragmentText, "[user]", playerName);
                break;

            case (FragmentStoryTellingSO.PlayerType.Hunter):
                foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    if (np.TypePlayerEnum == NetworkPlayer.TypePlayer.Hunter)
                        playerName = np.username;

                textFragment.text = SetBetween(currentFragment.fragmentText, "[user]", playerName);
                break;
            case (FragmentStoryTellingSO.PlayerType.Wiseman):
                foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
                    if (np.TypePlayerEnum == NetworkPlayer.TypePlayer.Wiseman)
                        playerName = np.username;

                textFragment.text = SetBetween(currentFragment.fragmentText, "[user]", playerName);
                break;
            case (FragmentStoryTellingSO.PlayerType.All):
                
                textFragment.text = currentFragment.fragmentText;
                break;

        }
    }
    public static string SetBetween(string strSource, string searchStr, string replaceStr)
    {
        if (strSource.Contains(searchStr))
        {
            int Start, End;
            int lenght = searchStr.Length;
          
            Start = strSource.IndexOf(searchStr.Substring(0));
            
            End = strSource.IndexOf(searchStr.Substring(lenght-1));

            Debug.Log(Start + "  " + End + " " + lenght);

            int totLenght = strSource.Length - End;
            string result = strSource.Substring(0,Start) + replaceStr + strSource.Substring(End+1 ,totLenght-1);
            return result;
        }

        return "string not found";
    }
}
