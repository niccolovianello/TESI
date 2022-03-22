using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRadar : MonoBehaviour
{

    [SerializeField] private float speed = 360f;
    [SerializeField] private SpriteRenderer trail;
    [SerializeField] private float lifetime = 2f;

    private bool _routineHasStarted = false;
    

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        
        if (!_routineHasStarted)
        {
            StartCoroutine(TrailFadeIn());
            _routineHasStarted = true;
        }
        
        transform.eulerAngles += new Vector3(0, speed * Time.deltaTime, 0);
        
    }

    private IEnumerator TrailFadeIn()
    {
        float _alpha = 0;
        
        
        while(_alpha < 1)
        {
            trail.color += new Color(0, 0, 0, _alpha);
            _alpha += 0.01f;
            yield return new WaitForSeconds(0.05f);
        }

        yield break;
    }
}
