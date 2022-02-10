using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HuntAndKillMazeAlgorithm : MazeAlgorithm
{
    private int currentRow = 0;
    private int currentColumn = 0;

    private bool courseComplete = false;

    public HuntAndKillMazeAlgorithm(MazeCell[,] mazeCells) : base(mazeCells)
    { 
    }

    public override void CreateMaze()
    {
        HuntAndKill();
    }

    private void HuntAndKill()
    {
        mazeCells[currentRow, currentColumn].visited = true;

        while (!courseComplete) {

            Kill();
            Hunt();
        }
    }

    private void Kill()
    {
        while (RouteStillAvailable(currentRow, currentColumn))
        {
            int direction = ProceduralNumberGenerator.GetNextNumber();

            if (direction == 1 && CellIsAvailable(currentRow - 1, currentColumn))
            {
                // north
                DestroyWallIfExists(mazeCells[currentRow, currentColumn].northWall); // overkill perchè si creano solo south wall e eastern wall
                DestroyWallIfExists(mazeCells[currentRow - 1, currentColumn].southWall);
                currentRow--;

            }
            else if (direction == 2 && CellIsAvailable(currentRow + 1, currentColumn))
            {
                //south
                DestroyWallIfExists(mazeCells[currentRow, currentColumn].southWall); // overkill perchè si creano solo south wall e eastern wall
                DestroyWallIfExists(mazeCells[currentRow + 1, currentColumn].northWall);
                currentRow++;

            }
            else if (direction == 3 && CellIsAvailable(currentRow, currentColumn+1))
            {
                //east
                DestroyWallIfExists(mazeCells[currentRow, currentColumn].eastWall); 
                DestroyWallIfExists(mazeCells[currentRow, currentColumn + 1].weastWall);// overkill perchè si creano solo south wall e eastern wall
                currentColumn++;

            }
            else if (direction == 4 && CellIsAvailable(currentRow + 1, currentColumn))
            {
                //weast
                DestroyWallIfExists(mazeCells[currentRow, currentColumn].weastWall); // overkill perchè si creano solo south wall e eastern wall
                DestroyWallIfExists(mazeCells[currentRow, currentColumn-1].eastWall);
                currentColumn--;

            }

            mazeCells[currentRow, currentColumn].visited = true;
        }
    }

    private void Hunt()
    {
        courseComplete = true;

        for (int r=0 ; r < mazeRows; r++)
        {
            for (int c = 0; c < mazeColumns; c++)
            {
                if (!mazeCells[r, c].visited && CellHasAnAsjacentVisitedCell(r, c))
                {
                    courseComplete = false;
                    currentRow = r;
                    currentColumn = c;
                    DestroyAdjacentWall(currentRow, currentColumn);
                    mazeCells[currentRow, currentColumn].visited = true;
                    return;
                }
            }
        }
    }

    private bool RouteStillAvailable(int row, int column)
    {
        int availableRoutes = 0;

        if (row > 0 && !mazeCells[row - 1, column].visited)
        {
            availableRoutes++;
        }
        if (row < mazeRows -1  && !mazeCells[row + 1, column].visited)
        {
            availableRoutes++;
        }
        if (column > 0 && !mazeCells[row, column - 1].visited)
        {
            availableRoutes++;
        }
        if (column > mazeColumns-1  && !mazeCells[row, column + 1].visited)
        {
            availableRoutes++;
        }

        return availableRoutes > 0;
    }

    private bool CellIsAvailable(int row, int column)
    {
        if (row >= 0 && row < mazeRows && column >= 0 && column < mazeColumns && !mazeCells[row, column].visited) // attenione al valore di visited
        {
            return true;
        }
        else
            return false;
    }

    private void DestroyWallIfExists(GameObject wall)
    {
        if (wall != null)
        {
            GameObject.Destroy(wall);
        }
    }

    private bool CellHasAnAsjacentVisitedCell(int row, int column)
    {
        int visitedCells = 0;

        // Guarda a nord se siamo oltre la r = 1
        if (row > 0 && mazeCells[row - 1, column].visited)
        {
            visitedCells++;
        }

        if (row < (mazeRows - 2) && mazeCells[row + 1, column].visited)
        {
            visitedCells++;
        }
        if (column > 0 && mazeCells[row, column-1].visited)
        {
            visitedCells++;
        }
        if (column < (mazeColumns - 2) && mazeCells[row, column + 1].visited)
        {
            visitedCells++;
        }
        return visitedCells > 0;
    }

    public void DestroyAdjacentWall(int row, int column)
    {
        bool wallDestroyed = false;

        while (!wallDestroyed)
        {
            int direction = ProceduralNumberGenerator.GetNextNumber();

            if (direction == 1 && row > 0 && mazeCells[row - 1, column].visited)
            {
                DestroyWallIfExists(mazeCells[row, column].northWall);
                DestroyWallIfExists(mazeCells[row - 1, column].southWall);
                wallDestroyed = true;

            }

            else if (direction == 2 && row < (mazeRows - 2) && mazeCells[row + 1, column].visited)
            {
                DestroyWallIfExists(mazeCells[row, column].southWall);
                DestroyWallIfExists(mazeCells[row + 1, column].northWall);
                wallDestroyed = true;
            }
            else if (direction == 3 && column > 0 && mazeCells[row, column - 1].visited)
            {
                DestroyWallIfExists(mazeCells[row, column].weastWall);
                DestroyWallIfExists(mazeCells[row , column -1].eastWall);
                wallDestroyed = true;
            }
            else if (direction == 4 && column <( mazeColumns -2 ) && mazeCells[row, column+1].visited)
            {
                DestroyWallIfExists(mazeCells[row, column].eastWall);
                DestroyWallIfExists(mazeCells[row, column+1].weastWall);
                wallDestroyed = true;
            }
        }
    }
}
