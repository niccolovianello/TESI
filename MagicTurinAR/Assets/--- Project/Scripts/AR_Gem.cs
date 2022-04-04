using System.Collections;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.VFX;

public class AR_Gem : MonoBehaviour
{

    private VisualEffect _explosion;

    private int _attempts;

    private void Start()
    {
        _attempts = Random.Range(2, 5);
        _explosion = GetComponentInChildren<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_attempts != 0) return;
        {
            Vibration.Vibrate();
            Explosion();
            _attempts = -1;
        }
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Spell"))
        {
            Debug.Log("hit");
            Vibration.VibratePeek();
            _attempts--;
        }
    }

    private void Explosion()
    {
        GetComponentInChildren<MeshRenderer>().enabled = false;
        _explosion.Play();
        StartCoroutine(GemDestruction());
    }

    private IEnumerator GemDestruction()
    {
        yield return new WaitForSeconds(.5f);
        _explosion.Stop();
        yield return new WaitForSeconds(.5f);
        FindObjectOfType<UIDestroyGem>().OpenBackToGameWindow(3);
        Destroy(gameObject);
    }
}
