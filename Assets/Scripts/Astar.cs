using System.Collections.Generic;
using UnityEngine;

public class Astar
{
    // Each Tile is a node in the grid
    Tile[,] _grid;
    public List<Tile> path;
    bool allowDiagonal = false;

    public bool FindPath(Tile[,] grid, Vector2 startPosition, Vector2 endPosition)
    {
        _grid = grid;
        Tile startTile = grid[(int)startPosition.x, (int)startPosition.y];
        Tile endTile = grid[(int)endPosition.x, (int)endPosition.y];

        List<Tile> openList = new List<Tile>();
        openList.Add(startTile);

        HashSet<Tile> closedList = new HashSet<Tile>();

        while (openList.Count > 0)
        {
            Tile currentTile = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentTile.fCost || openList[i].fCost == currentTile.fCost && openList[i].hCost < currentTile.hCost)
                {
                    currentTile = openList[i];
                }
            }

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == endTile)
            {
                RetracePath(startTile, endTile);
                return true;
            }

            foreach (Tile neighbour in GetNeighbours(currentTile))
            {
                if (!neighbour.isWalkable || closedList.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbout = currentTile.gCost + GetDistance(currentTile, neighbour);
                if (newMovementCostToNeighbout < neighbour.gCost || !openList.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbout;
                    neighbour.hCost = GetDistance(neighbour, endTile);
                    neighbour.parent = currentTile;

                    if (!openList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }
        }
        Debug.Log("Path not found!");
        return false;
    }

    void RetracePath(Tile start, Tile end)
    {
        path = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }

        path.Reverse();
    }

    public int GetDistance(Tile a, Tile b)
    {
        int distX = Mathf.Abs(a.x - b.x);
        int distY = Mathf.Abs(a.y - b.y);

        return distY + distX; // use this if you want to use the manhattan method

        if (allowDiagonal)
        {
            if (distX > distY)
            {
                return 14 * distY + 10 * (distX - distY);
            }

            return 14 * distX + 10 * (distY - distX);
        }

        if (distX > distY)
        {
            return distY + 1 * (distX - distY);
        }

        return distX + 1 * (distY - distX);
        
    }

    public Vector2 TileToVector2(Tile tile)
    {
        return new Vector2(tile.x, tile.y);
    }

    public List<Tile> GetNeighbours(Tile tile)
    {
        List<Tile> neighbours = new List<Tile>();

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (allowDiagonal)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }
                }
                else
                {
                    if (x == 0 && y == 0 || x == 1 && y == 1 || x == 1 && y == -1 || x == -1 && y == -1 || x == -1 && y == 1)
                    {
                        continue;
                    }
                }

                int checkX = tile.x + x;
                int checkY = tile.y + y;

                if (checkX >= 0 && checkX < _grid.GetLength(0) && checkY >= 0 && checkY < _grid.GetLength(1))
                {
                    neighbours.Add(_grid[checkX, checkY]);
                    //_grid[checkX, checkY].tile_go.GetComponent<SpriteRenderer>().color = Color.yellow;
                }
            }
        }
        return neighbours;
    }
}
