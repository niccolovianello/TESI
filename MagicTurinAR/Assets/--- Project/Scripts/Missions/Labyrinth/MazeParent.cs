using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeParent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MazeLoader>().InstantiateMaze();
    }

   
}
