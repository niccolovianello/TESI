using UnityEngine;

public class AR_Gem : MonoBehaviour
{

    [SerializeField] private int attempts;

    private void Start()
    {
        attempts = Random.Range(2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (attempts == 0)
        {
            // explosion
            // fx
            
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spell"))
        {
            Debug.Log("hit");
            attempts--;
        }
    }
}
