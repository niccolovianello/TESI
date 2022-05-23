using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicStoryTellingSO", menuName = "MagicItems/MagicStoryTellingSO")]
public class MagicStoryTellingSO : ScriptableObject
{
    public List<FragmentStoryTellingSO> fragments = new List<FragmentStoryTellingSO>();
}

