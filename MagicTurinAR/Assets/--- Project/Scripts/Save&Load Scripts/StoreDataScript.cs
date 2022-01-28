using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreDataScript : MonoBehaviour
{
    public int role = 0;

    public enum TextRole
    {

        None = 0,
        Explorer = 1,
        Wiseman = 2,
        Hunter = 3


    }

    public int GetRoleIntData()
    {
        return role;
    }
    public void SetRoleData(int _role) {

        role = _role;
    }
}
