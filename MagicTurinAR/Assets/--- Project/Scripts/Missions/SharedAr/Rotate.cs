using System.Collections;
using UnityEngine;
using System;

public class Rotate : MonoBehaviour
{

    public static event Action<string, int> Rotated = delegate { };

    private bool _coroutineAllowed;
    public int numberShown;


    // Start is called before the first frame update
    private void Start()
    {
        _coroutineAllowed = true;
        numberShown = 7;
    }

    private void OnMouseDown()
    {
        if (_coroutineAllowed)
        {
            StartCoroutine(RotateWheel());
        }

    }

    private IEnumerator RotateWheel()
    {
        _coroutineAllowed = false;

        for (var i = 0; i <= 11; i++)
        {
            transform.Rotate(-3f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);
        }

        _coroutineAllowed = true;

        numberShown += 1;

        if (numberShown > 9)
            numberShown = 0;

        Rotated(name, numberShown);
    }
}
