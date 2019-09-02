using UnityEngine;

public class Tile
{
    public float posX;
    public float posY;
    public bool isWall;

    public Tile(float x, float y, bool isWall)
    {
        posX = x;
        posY = y;
        this.isWall = isWall;
    }
}
