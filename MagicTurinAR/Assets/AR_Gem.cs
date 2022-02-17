using UnityEngine;
using UnityEngine.SceneManagement;
using MirrorBasics;
using NetworkPlayer = MirrorBasics.NetworkPlayer;


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
            GameManager gameManager = FindObjectOfType<GameManager>();
            NetworkPlayer networkPlayer = gameManager.networkPlayer;
            // explosion
            // fx           
            gameManager.DisableMainGame();
            networkPlayer.NotRenderPlayerBody();
            gameManager.PlayerCameraObject.SetActive(false);
            SceneManager.UnloadSceneAsync("AR_DestroyGem");
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
