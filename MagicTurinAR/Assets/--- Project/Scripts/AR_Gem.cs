using UnityEngine;

public class AR_Gem : MonoBehaviour
{

    private int attempts;

    private void Start()
    {
        attempts = Random.Range(2, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if (attempts == 0)
        {
            FindObjectOfType<UIDestroyGem>().OpenBackToGameWindow(3);
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