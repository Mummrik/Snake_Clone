using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Prefabs")]
    public GameObject wallPrefab;
    public GameObject headPrefab;
    public GameObject bodyPrefab;
    public GameObject foodPrefab;
    public Text appleCounter;
    [HideInInspector] public int appleCount;
    [HideInInspector] public GameObject fruit;

    [Header("Multipliers")]
    public int levelScale;
    public float snakeSpeed;
    [HideInInspector] public bool usePathfinding;

    public Tile[,] grid;
    Camera mainCamera;
    int height;
    int width;
    
    private void Awake()
    {
        instance = this;
        mainCamera = Camera.main;
        // If no prefab is set in editor try to load the default. If there is no default the assertion will print a error.
        if (headPrefab == null) { headPrefab = Resources.Load("Prefabs/Head") as GameObject; }
        Assert.IsNotNull(headPrefab, @"headPrefab is null, didn't find the prefab at path 'Prefabs/Head'.");

        if (wallPrefab == null) { wallPrefab = Resources.Load("Prefabs/Wall") as GameObject; }
        Assert.IsNotNull(wallPrefab, @"wallPrefab is null, didn't find the prefab at path 'Prefabs/Wall'.");

        if (foodPrefab == null) { foodPrefab = Resources.Load("Prefabs/Fruit") as GameObject; }
        Assert.IsNotNull(foodPrefab, @"foodPrefab is null, didn't find the prefab at path 'Prefabs/Fruit'.");

        if (bodyPrefab == null) { bodyPrefab = Resources.Load("Prefabs/Body") as GameObject; }
        Assert.IsNotNull(bodyPrefab, @"bodyPrefab is null, didn't find the prefab at path 'Prefabs/Body'.");

        if (appleCounter == null) { appleCounter = GameObject.Find("appleCounter").GetComponent<Text>(); }
        Assert.IsNotNull(appleCounter, @"appleCounter is null, couldn't find the Text component.");

        if (levelScale <= 0) { levelScale = 2; }    // assign a default value if there is none set in the editor
        if (snakeSpeed <= 0) { snakeSpeed = 10; }    // assign a default value if there is none set in the editor
    }

    public void StartGame(bool useAStar = false)
    {
        usePathfinding = useAStar;
        Instantiate(headPrefab);            // instantiate the snake gameobject
        GenerateLevel();                    // generate a new level
        fruit = Instantiate(foodPrefab);    // set the fruit gameobject to the food prefab object, maybe i should just use the foodprefab object instead?
        fruit.name = "fruit";               // just set the name of the object to get rid of the (clone) thing in the hierarchy
        SpawnFruit();                       // just set a random position for the fruit instead of position (0,0)
        GameObject.Find("Canvas/Menu").SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    private void GenerateLevel()
    {
        width = 16 * levelScale + (levelScale * 2); // set the width of the game level, and use a scale to make a larger level
        height = 9 * levelScale + (levelScale);     // set the height of the game level, and use a scale to make a larger level
        mainCamera.orthographicSize = mainCamera.orthographicSize * levelScale + (levelScale * 0.1f);   // set the size of the camera depending of the level size
        mainCamera.transform.position = new Vector3(width / 2 - 0.5f, height / 2 - 0.5f, -10);          // set the position of the camera to be in the center of the screen

        grid = new Tile[width, height]; // set up a grid containing every tile on the level
        GameObject walls = new GameObject("Walls"); // setup a parent object for every wall in the game. mainly to clean up the hierarchy in the editor

        // here the generation of the level start.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (y <= 0 && x >= 0 || y <= height - 1 && x >= width - 1 || y >= 0 && x <= 0 || y >= height - 1 && x <= width - 1)
                {
                    // this will yield a wall object, and set the tile in the grid non walkable
                    GameObject go = Instantiate(wallPrefab, walls.transform);
                    go.transform.position = new Vector3(x, y, 0);
                    grid[x, y] = new Tile(x, y, false);
                }
                else
                {   // else the tile is walkable
                    grid[x, y] = new Tile(x, y, true);
                }
            }
        }
    }

    public void SpawnFruit()
    {
        int x;
        int y;

        // Check if the new position for the fruit is walkable, and not occupied by the snake already.
        // if the position is occupied it will just generate a new value for x and y untill its walkable
        do
        {
            x = Random.Range(1, width - 1);
            y = Random.Range(1, height - 1);

        } while (!grid[x, y].isWalkable);

        fruit.transform.position = new Vector3(x, y, 0);    // set the new position of the fruit
        appleCounter.text = $"Score: {appleCount.ToString()}";  // Update the score in game
    }
}
