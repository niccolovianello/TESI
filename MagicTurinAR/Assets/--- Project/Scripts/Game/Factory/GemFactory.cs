using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GemFactory : ItemFactory
{
    private List<Item> aliveGems= new List<Item>();
    public List<Item> AliveGems=> aliveGems;
    
}
