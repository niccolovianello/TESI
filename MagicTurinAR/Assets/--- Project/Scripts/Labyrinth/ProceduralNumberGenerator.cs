using System.Collections;
using UnityEngine;

public class ProceduralNumberGenerator
{
    public static int currentPosition;
    public const string key = "1423413223411342314231434132444132423142323";

    public static int GetNextNumber()
    {
        string currentNum = key.Substring(currentPosition++ % key.Length, 1);
        //return int.Parse(currentNum); // se vogliamo riprodurre sempre lo stesso labirinto

        return Random.Range(1, 5);
    }
}
