using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FragmentStoryTelling", menuName = "MagicItems/FragmentStoryTelling")]
public class FragmentStoryTellingSO : ScriptableObject
{
    public enum PlayerType
    {
        Explorer,
        Hunter,
        Wiseman,
        All

    }

    public int index;
    public string narratorText;
    public string fragmentTitle;
    public string fragmentText;

    public Sprite fragmentImage;

    public PlayerType fragmentDestinataryType;

  

}

