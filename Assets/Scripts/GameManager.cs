using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public LinkedList<GameObject> linkedList;

    public GameObject wallPrefab;
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject foodPrefab;
    public int levelScale;
    public float snakeSpeed;

    Tile[,] grid;
    Camera mainCamera;
    int wallPixelXSize;
    int wallPixelYSize;
    int height;
    int width;
    GameObject fruit;

    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        // If no prefab is set in editor load the default.
        if (headPrefab == null) { headPrefab = Resources.Load("Prefabs/Head") as GameObject; }
        if (wallPrefab == null) { wallPrefab = Resources.Load("Prefabs/Wall") as GameObject; }
        if (foodPrefab == null) { foodPrefab = Resources.Load("Prefabs/Fruit") as GameObject; }
        if (bodyPrefab == null) { bodyPrefab = Resources.Load("Prefabs/Body") as GameObject; }
        if (levelScale <= 0){ levelScale = 2; }
        if (snakeSpeed <= 0) { snakeSpeed = 5; }
        wallPixelXSize = wallPrefab.GetComponent<SpriteRenderer>().sprite.texture.width;
        wallPixelYSize = wallPrefab.GetComponent<SpriteRenderer>().sprite.texture.height;

        linkedList = new LinkedList<GameObject>();
        linkedList.addFirst(Instantiate(headPrefab));
        GenerateLevel();
        fruit = Instantiate(foodPrefab);
        SpawnFruit();
    }

    private void GenerateLevel()
    {
        width = 16 * levelScale + (levelScale * 2);
        height = 9 * levelScale + (levelScale);
        float cameraPosX = mainCamera.transform.position.x * levelScale;
        float cameraPosY = mainCamera.transform.position.y * levelScale;
        mainCamera.orthographicSize = mainCamera.orthographicSize * levelScale + (levelScale * 0.1f);
        mainCamera.transform.position = new Vector3(width / 2 - 0.5f, height / 2 - 0.5f, -10);

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
                    grid[x, y] = new Tile(x, y, true);
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

        fruit.transform.position = new Vector3(rndX, rndY, 0);
        fruit.name = "fruit";
    }
}
