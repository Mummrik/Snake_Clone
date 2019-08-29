using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject wallPrefab;
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject foodPrefab;

    Tile[,] grid;
    Camera camera;
    int wallPixelXSize;
    int wallPixelYSize;
    int height;
    int width;

    private void Awake()
    {
        instance = this;
        camera = Camera.main;
        // If no prefab is set in editor load the default.
        if (headPrefab == null) { headPrefab = Resources.Load("Prefabs/Head") as GameObject; }
        if (wallPrefab == null) { wallPrefab = Resources.Load("Prefabs/Wall") as GameObject; }
        if (foodPrefab == null) { foodPrefab = Resources.Load("Prefabs/Fruit") as GameObject; }
        wallPixelXSize = wallPrefab.GetComponent<SpriteRenderer>().sprite.texture.width;
        wallPixelYSize = wallPrefab.GetComponent<SpriteRenderer>().sprite.texture.height;
        Instantiate(headPrefab);
        GenerateLevel();
        SpawnFruit();
    }

    private void GenerateLevel()
    {
        height = camera.pixelHeight / wallPixelYSize - 2;
        width = camera.pixelWidth / wallPixelXSize - 4;

        grid = new Tile[width, height];
        GameObject walls = new GameObject("Walls");
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y <= 0 && x >= 0 || y <= height - 1 && x >= width - 1 || y >= 0 && x <= 0 || y >= height - 1 && x <= width - 1)
                {
                    GameObject go = Instantiate(wallPrefab, walls.transform);
                    go.transform.position = new Vector3(x, y, 0);
                    go.GetComponent<Tile>().posX = x;
                    go.GetComponent<Tile>().posY = y;
                    grid[x, y] = go.GetComponent<Tile>();
                }
                else
                {
                    grid[x, y] = new Tile(x, y, false);
                }

            }
        }
    }

    public void SpawnFruit()
    {
        int rndX = Random.Range(1, width - 1);
        int rndY = Random.Range(1, height - 1);

        GameObject fruit = Instantiate(foodPrefab);
        fruit.transform.position = new Vector3(rndX, rndY, 0);
        fruit.name = "fruit";
    }
}
