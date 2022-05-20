using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManagerSharedAR : MonoBehaviour
{

    [Header("ReferenceToCamera")]
    [SerializeField] private List<Component> componentsToDisable = new List<Component>();

    public void DestroyNotHostComponents()
    {
        foreach (Component comp in componentsToDisable)
        {
            comp.Destroy();
        }
        this.Destroy();
    }
}
