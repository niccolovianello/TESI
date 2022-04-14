using System.Collections;
using UnityEngine;
using System;
using NetworkPlayer = MirrorBasics.NetworkPlayer;

public class Rotate : MonoBehaviour
{

    public static event Action<string, int> Rotated = delegate { };

    private bool _coroutineAllowed;
    public int numberShown;
    private NetworkPlayer np;


    // Start is called before the first frame update
    private void Start()
    {
        _coroutineAllowed = true;
        numberShown = 7;

        
        foreach (NetworkPlayer np in FindObjectsOfType<NetworkPlayer>())
            if (np.isLocalPlayer)
                this.np = np;
    }


    private void OnMouseDown()
    {
        if (_coroutineAllowed)
        {
            //StartCoroutine(RotateWheelCoroutine());
            np.CmdRotateLockWheel(name);
        }
    }

    public void RotateWheel()
    {
        StartCoroutine(RotateWheelCoroutine());
    }


    private IEnumerator RotateWheelCoroutine()
    {
        _coroutineAllowed = false;

        for (var i = 0; i <= 11; i++)
        {
            transform.Rotate(3f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        _coroutineAllowed = true;

        numberShown += 1;

        if (numberShown > 9)
            numberShown = 0;

        Rotated(name, numberShown);
    }
}