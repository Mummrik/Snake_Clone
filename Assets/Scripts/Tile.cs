using UnityEngine;

public class Tile : MonoBehaviour
{
    public float posX;
    public float posY;
    public bool isWall;

    public Tile(float x, float y, bool isWall = true)
    {
        posX = x;
        posY = y;
        isWall = isWall;
    }
}
