using System;
using System.Collections.Generic;
using UnityEngine;

public class Astar : MonoBehaviour
{
    // usefull info https://codereview.stackexchange.com/questions/114512/simple-pathfinding-algorithm
    Stack<Tile> stack;
    Tile[,] map;
    Vector3 startPosition;
    Vector3 currentPosition;
    Vector3 endPosition;

    public Astar(Tile[,] grid, Vector3 startPosition, Vector3 endPosition)
    {
        map = grid;
        this.startPosition = startPosition;
        this.endPosition = endPosition;
        stack = new Stack<Tile>();
    }

    public void FindPath(Vector3 start, Vector3 end)
    {
        if (start == end)
        {
            Debug.Log("Path was found");
            return;
        }

        foreach (var tile in map)
        {
            if (tile.isWall == true || stack.Count != 0 && stack.Contains(tile)) { continue; }

            stack.Push(map[(int)start.x, (int)start.y]);
            FindPath(new Vector3(tile.posX, tile.posY, 0), end);
        }
        stack.Pop();
    }

    private void MoveToLocation()
    {

    }
}
