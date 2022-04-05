using System.Collections.Generic;
using UnityEngine;

public class MemoryManager : MonoBehaviour
{
    private Transform[] spawnPoints;
    
    // This flag is 0 when there isn't any uncovered memory card.
    // The value is 1 when there is an uncovered card waiting for a match.
    private int _uncoveredCardFlag;
    private MemoryCard _oldCard = null;
    private List<MemoryCard> _cardInstances;
    private int _counter;

    [SerializeField] private List<MemoryCard> cards;

    // Start is called before the first frame update
    private void Start()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
        _cardInstances = new List<MemoryCard>();

        // first card instances
        foreach (MemoryCard card in cards)
        {
            // index starts from 1 in order to ignore the parent object
            int index = Random.Range(1, spawnPoints.Length - 1);

            Transform sp = spawnPoints[index];
            Instantiate(card, sp.position, sp.rotation);
            _cardInstances.Add(card);

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
        
        _counter = _cardInstances.Count;
       
    }

    public void CheckMatch(MemoryCard newCard)
    {

        if (_uncoveredCardFlag == 1)
        {
            _uncoveredCardFlag = 0;
            
            if (newCard.CardId == _oldCard.CardId)
            {
                
                Destroy(_oldCard.gameObject);
                Destroy(newCard.gameObject);

                _counter--;

                if (_counter == 0)
                {
                    FindObjectOfType<MissionsManager>().OpenFinishMissionWindow();
                }
            }

            else
            {
                _oldCard.Animation.Play("Coprire");
                newCard.Animation.Play("Coprire");
                _oldCard.Collider.enabled = true;
                newCard.Collider.enabled = true;
            }
            
        }

        else
        {
            _oldCard = newCard;
            _oldCard.Collider.enabled = false;
        
            _uncoveredCardFlag = 1;
        }
        
    }
    
    
}
