using System.Collections;
using UnityEngine;

public class ProceduralNumberGenerator
{
    public static int currentPosition;
    public const string key = "1212341445345456235652355454632425";

    public static int GetNextNumber()
    {
        string currentNum = key.Substring(currentPosition++ % key.Length, 1);

        return Random.Range(1, 5);
    }
}
