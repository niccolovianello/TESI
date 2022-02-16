using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    private Transform[] spawnPoints;
    
    // This flag is 0 when there isn't any uncovered memory card.
    // The value is 1 when there is an uncovered card waiting for a match.
    private int uncoveredCardFlag;
    private MemoryCard oldCard = null;
    private List<MemoryCard> cardInstances;

    [SerializeField] private List<MemoryCard> cards;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        cardInstances = new List<MemoryCard>();

        // first card instances
        foreach (MemoryCard card in cards)
        {
            // index starts from 1 in order to ignore the parent object
            int index = Random.Range(1, spawnPoints.Length - 1);

            Transform sp = spawnPoints[index];
            Instantiate(card, sp.position, sp.rotation);
            cardInstances.Add(card);

            List<Transform> tempSpawnPointList = new List<Transform>(spawnPoints);
            tempSpawnPointList.RemoveAt(index);
            spawnPoints = tempSpawnPointList.ToArray();

        }
        
        // card duplicates
        foreach (MemoryCard card in cards)
        {
            int index = Random.Range(1, spawnPoints.Length - 1);

            Transform sp = spawnPoints[index];
            Instantiate(card, sp.position, sp.rotation);

            List<Transform> tempSpawnPointList = new List<Transform>(spawnPoints);
            tempSpawnPointList.RemoveAt(index);
            spawnPoints = tempSpawnPointList.ToArray();

        }
       
    }

    public void CheckMatch(MemoryCard newCard)
    {

        if (uncoveredCardFlag == 1)
        {
            uncoveredCardFlag = 0;
            
            if (newCard.CardId == oldCard.CardId)
            {
                // there is a match!
                // SFX, VFX...

                /*
                 
                 not working!!
                 
                foreach (MemoryCard card in cardInstances)
                {
                    if (card.CardId == newCard.CardId)
                    {
                        cardInstances.Remove(card);
                    }
                }
                 */
                
                Debug.Log(cardInstances.Count);
                Destroy(oldCard.gameObject);
                Destroy(newCard.gameObject);

                if (cardInstances.Count == 0)
                {
                    // task completed
                    Debug.Log("Hai Vinto!");
                }
            }

            else
            {
                oldCard.Animation.Play("Coprire");
                newCard.Animation.Play("Coprire");
                oldCard.Collider.enabled = true;
                newCard.Collider.enabled = true;
            }
            
        }

        else
        {
            oldCard = newCard;
            oldCard.Collider.enabled = false;
        
            uncoveredCardFlag = 1;
        }
        
    }
    
    
}
