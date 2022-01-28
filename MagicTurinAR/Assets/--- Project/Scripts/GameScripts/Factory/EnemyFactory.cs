using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyFactory : Singleton<EnemyFactory>
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Enemy enemyPrefabScript;
    [SerializeField] private MagicPlayer player;
    [SerializeField] private float waitTime = 10.0f;
    [SerializeField] private int startingEnemies = 10;
    [SerializeField] private float minRange = 5.0f;
    [SerializeField] private float maxRange = 50.0f;
    [SerializeField] private GameObject parentObjectsFactory;

    private List<Enemy> aliveEnemies = new List<Enemy>();
    private Enemy selectedEnemy;

    public List<Enemy> AliveEnemies => aliveEnemies;

    public Enemy SelectedEnemy => selectedEnemy;

    

    private void Awake()
    {
       
    }

    void Start()
    {
        player = FindObjectOfType<MagicPlayer>();
        parentObjectsFactory = GameObject.Find("ParentEnemies");
        for (int i = 0; i < startingEnemies; i++)
        {
            InstantiateEnemy();
        }

        StartCoroutine(GenerateEnemy());

        //Assert.IsNotNull(enemyPrefab);
        //Assert.IsNotNull(player);

        
        Debug.Log(parentObjectsFactory);

    }

    public void EnemyWasSelected(Enemy i)
    {
        selectedEnemy = i;
    }

    private IEnumerator GenerateEnemy()
    {
        while (true)
        {
            InstantiateEnemy();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void InstantiateEnemy()
    {
        float x = player.transform.position.x + GenerateRange();
        float y = player.transform.position.y;
        float z = player.transform.position.z + GenerateRange();
        
        GameObject enemyToIstantiate = Instantiate(enemyPrefab, new Vector3(x, y, z), Quaternion.identity);
        
        enemyToIstantiate.transform.parent = parentObjectsFactory.transform;


        aliveEnemies.Add(enemyToIstantiate.GetComponent<Enemy>());
    }

    private float GenerateRange()
    {
        float rand = Random.Range(minRange, maxRange);
        bool isPositive = Random.Range(0, 10) < 5;
        return rand * (isPositive ? 1 : -1);
    }

    public void SetEnemyPrefab(GameObject enemy)
    {
        enemyPrefab = enemy;    
    }

    public void SetMagicPlayer(MagicPlayer magicPlayer)
    {
        player = magicPlayer;
    }
}
