using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rotate : MonoBehaviour
{

    public static event Action<string, int> Rotated = delegate { };

    private bool coroutineAllowed;
    public int numberShown;


    // Start is called before the first frame update
    void Start()
    {
        coroutineAllowed = true;
        numberShown = 5;

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (coroutineAllowed)
        {
            StartCoroutine(RotateWheel());
        }

    }

    private IEnumerator RotateWheel()
    {
        coroutineAllowed = false;

        for (int i = 0; i <= 11; i++)
        {
            transform.Rotate(-3f, 0f, 0f);
            yield return new WaitForSeconds(0.01f);

        }

        coroutineAllowed = true;

        numberShown += 1;

        if (numberShown > 9)
            numberShown = 0;

        Rotated(name, numberShown);
    }
}
