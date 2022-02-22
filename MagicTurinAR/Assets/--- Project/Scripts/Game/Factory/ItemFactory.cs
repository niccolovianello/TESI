using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class ItemFactory : Singleton<ItemFactory>
{
    [SerializeField] public float waitTime;
    [SerializeField] public int startingItems;
    [SerializeField] public float minRange;
    [SerializeField] public float maxRange;
    
    [SerializeField] public GameObject itemPrefab;

    [SerializeField] public Player player;
    
    //TEST
   // public GameObject rif;
    
    [SerializeField] private GameObject parentObjectsFactory;

    private List<Item> aliveItems = new List<Item>();

    void Start()
    {
        minRange = 5;
        maxRange = 20;
        waitTime = 30f;
        player = FindObjectOfType<MagicPlayer>();
        parentObjectsFactory = GameObject.Find("ParentItems");
        for (int i = 0; i < startingItems; i++)
        {
            InstantiateItem();
        }
        
        StartCoroutine(GenerateItem());
    }

    private IEnumerator GenerateItem()
    {
        while (true)
        {
            InstantiateItem();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void InstantiateItem()
    {
        if (aliveItems.Count >= 10)
            aliveItems.RemoveAt(0);
        //float x = player.transform.position.x + GenerateRange();
        //float y = player.transform.position.y;
        //float z = player.transform.position.z + GenerateRange();
        
        float x = player.transform.position.x + GenerateRange();
        float y = player.transform.position.y + 2f;
        float z = player.transform.position.z + GenerateRange();

        GameObject itemToIstantiate = Instantiate(itemPrefab, new Vector3(x, y, z), Quaternion.identity);
        itemToIstantiate.transform.parent = parentObjectsFactory.transform;

        aliveItems.Add(itemToIstantiate.GetComponent<Item>());
    }
    
    private float GenerateRange()
    {
        float rand = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return rand * (isPositive ? 1 : -1);
    }

    public void SetItemPrefab(GameObject prefabGO)
    {
        itemPrefab = prefabGO;
    }

    public void SetMagicPlayer(MagicPlayer magicPlayer)
    {
        player = magicPlayer;
    }

}