using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollectObject : MonoBehaviour
{
    public abstract void playerTabbed(GameObject player);
    public abstract void collectableTabbed(GameObject collectable);
}
