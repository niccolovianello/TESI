using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    private Transform[] spawnPoints;
    
    [SerializeField] private List<MemoryCard_ScriptableObject> cards;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        
        Debug.Log(spawnPoints.Length);

        foreach (MemoryCard_ScriptableObject card in cards)
        {
            int index = Random.Range(1, spawnPoints.Length - 1);
            //Debug.LogWarning(spawnPoints.Length);
            Transform sp = spawnPoints[index];
            Instantiate(card.card, sp.position, sp.rotation);
            //Debug.Log(card.id + " " + sp.position);

            List<Transform> tempSpawnPointList = new List<Transform>(spawnPoints);
            tempSpawnPointList.RemoveAt(index);
            spawnPoints = tempSpawnPointList.ToArray();
            
            //Debug.Log(spawnPoints);
        }

        foreach (MemoryCard_ScriptableObject card in cards)
        {
            int index = Random.Range(1, spawnPoints.Length - 1);
            //Debug.LogWarning(index);
            Transform sp = spawnPoints[index];
            Instantiate(card.card, sp.position, sp.rotation);
            //Debug.Log(card.id + " " + sp.position);

            List<Transform> tempSpawnPointList = new List<Transform>(spawnPoints);
            tempSpawnPointList.RemoveAt(index);
            spawnPoints = tempSpawnPointList.ToArray();

        }
       
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
    
    
}
