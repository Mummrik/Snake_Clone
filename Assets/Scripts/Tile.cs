using UnityEngine;

public class Tile
{
    public int x;
    public int y;
    public bool isWalkable;

    public Tile parent;

    public int gCost;
    public int hCost;
    public int fCost { get => gCost + hCost; }

    public Tile(int x, int y, bool isWall)
    {
        this.x = x;
        this.y = y;
        isWalkable = isWall;
    }
}
