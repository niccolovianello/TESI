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
        FindObjectOfType<UILobby>().gameObject.SetActive(false);
    }

    private void SetFragment()
    {
        indexFragment.text = (currentFragmentIndex + 1) + " / " + storyTellingSO.fragments.Count;
        imageFragment.sprite = currentFragment.fragmentImage;
        narratorFragment.text = currentFragment.narratorText;
        titleFragment.text = currentFragment.fragmentTitle;
        textFragment.text = currentFragment.fragmentText;
    }
}
