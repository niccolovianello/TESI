using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceBasedShading : MonoBehaviour
{
    
    public Transform Player;

    private MeshRenderer[] _meshRenderers;
    [SerializeField] private float distance;

    // Update is called once per frame
    void Update()
    {
        foreach (MeshRenderer meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            if ((Player.position - meshRenderer.gameObject.transform.position).magnitude < distance)
            {
                meshRenderer.material.color = Color.blue;
            }
            
            else meshRenderer.material.color = Color.red;
        }
    }
}
