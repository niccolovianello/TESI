using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreData : MonoBehaviour
{
    public TextRole role;

    public enum TextRole
    {


        Explorer,
        Wiseman,
        Hunter


    }

    public int GetRoleIntData()
    {
        return (int)role;
    }
    public void SetRoleData(int _role)
    {

        role =(TextRole) _role;
        
    }
}
