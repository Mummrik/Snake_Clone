using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    //Public variables
    public enum Direction { up, left, down, right }
    public Direction snakeDirection;
    public bool gameOver;
    public float gameTick;
    public float gameSpeedMultiplier;

    //private variables
    Vector3 snakeMovement;
    int growSnake;
    Vector3 snakeLastPosition;
    LinkedList<GameObject> linkedList;

    // A*
    public bool usePathfinding = true;
    Astar pathfinding;

    //Using a start instead of awake to make the GameManager setup the game area before we populate it
    private void Start()
    {
        Camera camera = Camera.main;
        transform.position = new Vector3(camera.transform.position.x - 0.5f, camera.transform.position.y - 0.5f, transform.position.z);
        snakeMovement = transform.position + Vector3.up * (0.01f * gameSpeedMultiplier);
        gameObject.name = "Snake";
        gameSpeedMultiplier = GameManager.instance.snakeSpeed;
        linkedList = new LinkedList<GameObject>();
        growSnake = 3;   // always start whit 3 body parts
        if (usePathfinding)
        {
            pathfinding = new Astar(GameManager.instance.grid, transform.position, GameManager.instance.fruit.transform.position);
        }
    }
    private void Update()
    {
        if (gameOver)
        {
            SceneManager.LoadScene(0);
        }
        if (gameTick > 1 / gameSpeedMultiplier)
        {
            MoveSnake();    // move the snake
            GrowSnake();    // after the snake moved grow it
            RotateSnake(snakeDirection);    // set the rotation of the snake head.
        }

        SetSnakeMoveDirection();
        gameTick += Time.fixedDeltaTime;    // increase the tick timer to manipulate the game speed/ movement speed
    }

    private void SetSnakeMoveDirection()
    {
        // set the game speed when 10 fruits have been collected
        if (GameManager.instance.snakeSpeed >= gameSpeedMultiplier + 1) { gameSpeedMultiplier++; }

        // input to change the direction of the snake
        if (Input.GetKeyDown(KeyCode.UpArrow) && snakeDirection != Direction.down) { snakeDirection = Direction.up; }
        if (Input.GetKeyDown(KeyCode.DownArrow) && snakeDirection != Direction.up) { snakeDirection = Direction.down; }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && snakeDirection != Direction.right) { snakeDirection = Direction.left; }
        if (Input.GetKeyDown(KeyCode.RightArrow) && snakeDirection != Direction.left) { snakeDirection = Direction.right; }

        //set the movement of the snake
        switch (snakeDirection)
        {
            case Direction.up:
                snakeMovement = transform.position + Vector3.up;
                break;
            case Direction.down:
                snakeMovement = transform.position + Vector3.down;
                break;
            case Direction.left:
                snakeMovement = transform.position + Vector3.left;
                break;
            case Direction.right:
                snakeMovement = transform.position + Vector3.right;
                break;
            default:
                break;
        }
    }

    private void MoveSnake()
    {
        // move the snake
        gameTick = 0;
        snakeLastPosition = transform.position;  // store the last position of the snake head
        transform.position = snakeMovement;      // move the snake head to its new position

        if (linkedList.count() > 0)
        {
            // Make the body part follow the head, or its previous node in the linkedList
            GameObject temp = linkedList.getLast(); // get the last element in the linked list in a temp gameobject
            GameManager.instance.grid[(int)temp.transform.position.x, (int)temp.transform.position.y].isWall = false;    // set the tile to a walkable tile
            linkedList.remove(linkedList.getLast());    // remove the last element in the list
            temp.transform.position = snakeLastPosition;     // set the position of the temp gameobject to the heads last position
            linkedList.addFirst(temp);                  // put it back in the linked list as the second element in the linked list, behind the head/root element
        }
    }

    private void GrowSnake()
    {
        // if the snake should grow its length
        if (growSnake > 0)
        {
            growSnake--; // decrease the grow variable by 1 to make sure it wont spawn more elements than its suposed to
            GameObject body = Instantiate(GameManager.instance.bodyPrefab, snakeLastPosition, Quaternion.identity);  // create the gameobject in the game scene
            GameManager.instance.grid[(int)body.transform.position.x, (int)body.transform.position.y].isWall = true;    // set the tile to a blocking tile
            body.name = "body"; // just rename it, to make it look clean in the hierarcy
            linkedList.addFirst(body);  // add the gameobject to the first element in the linked list,
            //(Head gameobject is not really inside the linked list it just contain the list, and all the body parts is inside the list. Following the position of the head)
        }
    }

    private void RotateSnake(Direction direction)
    {
        // set the rotation of the snake
        transform.rotation = Quaternion.Euler(0, 0, (float)direction * 90f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" || collision.tag == "Body")
        {
            // if a wall or a body part is hit by the head set the game to gameover and restart the scene
            gameOver = true;
        }
        if (collision.tag == "Fruit")
        {
            // a fruit is collected make the grow variable increase, to spawn a new body part
            growSnake++;
        }
    }
}
