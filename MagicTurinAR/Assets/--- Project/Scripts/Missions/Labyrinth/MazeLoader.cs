using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Niantic.ARDKExamples.Helpers;

public class MazeLoader : MonoBehaviour
{
    public int mazeRows = 12, mazeColumns = 18;
    public GameObject wall;
    public float size = 6f;
    public Material floorMaterial;
    public GameObject MazeParent;
    public float MazeScaleFactor = 0.01f;
    public GameObject Player;
    public GameObject MazeGoal;


    public MazeCell[,] mazeCells;
   

    public void InstantiateMaze()
    {
        MazeParent = FindObjectOfType<MazeParentScript>().gameObject;
        InitializeMaze();
        MazeParent.transform.localScale *= MazeScaleFactor;
        MazeAlgorithm ma = new HuntAndKillMazeAlgorithm(mazeCells);
        ma.CreateMaze();

        GameObject player = Instantiate(Player, MazeParent.transform);
        player.transform.localPosition = new Vector3(mazeCells[0,0].floor.transform.localPosition.x, 0.1f, mazeCells[0,0].floor.transform.localPosition.z);
        player.GetComponent<PlayerMazeMovement>().InitializeMazePlayerController();

        GameObject mazeGoal = Instantiate(MazeGoal, MazeParent.transform);
        mazeGoal.transform.localPosition = new Vector3(mazeCells[mazeRows - 1, mazeColumns - 1].floor.transform.localPosition.x, 0.1f, mazeCells[mazeRows - 1, mazeColumns - 1].floor.transform.localPosition.z);



    }



    private void InitializeMaze()
    {
        mazeCells = new MazeCell[mazeRows, mazeColumns];

        for (int r = 0; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                mazeCells[r, c] = new MazeCell();

                mazeCells[r, c].floor = Instantiate(wall, new Vector3(r * size, -(size / 2f), c * size), Quaternion.identity);
                mazeCells[r, c].floor.name = "Floor " + r + ", " + c;
                mazeCells[r, c].floor.transform.Rotate(Vector3.right, 90f);
                MeshRenderer meshR = mazeCells[r, c].floor.GetComponent<MeshRenderer>();
                meshR.material = floorMaterial;
                mazeCells[r, c].floor.transform.SetParent(MazeParent.transform);

                if (c == 0)
                {
                    mazeCells[r, c].weastWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) - (size / 2f)), Quaternion.identity);
                    mazeCells[r, c].weastWall.name = "West Wall " + r + ", " + c;
                    mazeCells[r, c].weastWall.transform.SetParent(MazeParent.transform);
                }

                mazeCells[r, c].eastWall = Instantiate(wall, new Vector3(r * size, 0, (c * size) + (size / 2f)), Quaternion.identity);
                mazeCells[r, c].eastWall.name = "East Wall " + r + ", " + c;
                mazeCells[r, c].eastWall.transform.SetParent(MazeParent.transform);

                if (r == 0)
                {
                    mazeCells[r, c].northWall = Instantiate(wall, new Vector3((r * size) - (size / 2f), 0, c * size), Quaternion.identity);
                    mazeCells[r, c].northWall.name = "North Wall " + r + ", " + c;
                    mazeCells[r, c].northWall.transform.Rotate(Vector3.up, 90f);
                    mazeCells[r, c].northWall.transform.SetParent(MazeParent.transform);
                }

                mazeCells[r, c].southWall = Instantiate(wall, new Vector3((r * size) + (size / 2f), 0, c * size), Quaternion.identity);
                mazeCells[r, c].southWall.name = "South Wall " + r + ", " + c;
                mazeCells[r, c].southWall.transform.Rotate(Vector3.up, 90f);
                mazeCells[r, c].southWall.transform.SetParent(MazeParent.transform);


            }

        }
    }

    
}
