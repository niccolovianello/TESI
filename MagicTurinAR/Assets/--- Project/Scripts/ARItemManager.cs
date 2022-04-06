using System.Collections;
using System.Collections.Generic;
using Niantic.ARDKExamples.Helpers;
using UnityEngine;

public class ARItemManager : MonoBehaviour
{
    public CustomARHitTest arHitTester;
    private GameObject collectablePrefab;
    protected Plane Plane;
    private Camera Camera;

    public float _currentScale, _scaleRate, _temp;

    public float minScale, maxScale;

    public GameObject CollectablePrefab
    {
        get => collectablePrefab;
        set => collectablePrefab = value;
    }


    // Start is called before the first frame update
    void Start()
    {
        GameObject gameManager = GameObject.Find("Loader");
        GameManager gm = gameManager.GetComponent<GameManager>();
        Camera = FindObjectOfType<Camera>();
        //arHitTester = GetComponent<CustomARHitTest>();
        arHitTester.PlacementObjectPf = gm.PrefabToShowInAr;
        _currentScale = collectablePrefab.transform.localScale.x;
    }

    void Update()
    {
        if (Input.touchCount == 2 && arHitTester.flagInteraction)
        {
            collectablePrefab.transform.localScale = new Vector3(_currentScale, _currentScale, _currentScale);

            float distance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);

            if (_temp > distance)
            {
                if (_currentScale < minScale)
                    return;
                _currentScale -= (Time.deltaTime) * _scaleRate;
            }
            else if (-_temp < distance)
            {
                if (_currentScale >= maxScale)
                    return;
                _currentScale += (Time.deltaTime) * _scaleRate;
            }
            _temp = distance;

        }
    }
}

   