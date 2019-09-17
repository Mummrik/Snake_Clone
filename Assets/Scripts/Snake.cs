using System;
using System.Collections.Generic;
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
    bool hasPath;

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
        usePathfinding = GameManager.instance.usePathfinding;
    }

    private void Update()
    {
        if (!usePathfinding)
        {
            // this take care of the player input to set the snake move direction
            SetSnakeMoveDirection();
        }
    }

    private void FixedUpdate()
    {
        if (gameOver)
        {
            // if the game is over just reload the scene
            SceneManager.LoadScene(0);
            //return;
        }

        // make sure the game has a speed limit so it wont run to fast
        if (gameTick > 1 / gameSpeedMultiplier)
        {
            // make sure to reset the gametick
            gameTick = 0;

            // stuff related to the Astar pathfinding
            if (usePathfinding)
            {
                // if the pathfinding is null, assign a new Astar instance
                if (pathfinding == null)
                {
                    pathfinding = new Astar();
                }
                // generate a new path to the fruit
                hasPath = pathfinding.FindPath(GameManager.instance.grid, transform.position, GameManager.instance.fruit.transform.position);
                // if a path is found the snake will find its way to the fruit
                if (hasPath)
                {
                    PathfindSnake();
                }
                else
                {
                    // if there is no path to the fruit, the snake will try to find a way to its tail. Or just take a random path depending on if its neighbour is walkable
                    bool tailPath = false;
                    //GameManager.instance.grid[(int)linkedList.getLast().transform.position.x, (int)linkedList.getLast().transform.position.y].isWalkable = true;
                    //tailPath = pathfinding.FindPath(GameManager.instance.grid, transform.position, linkedList.getLast().transform.position);
                    // uncomment the 2 above lines if you want the snake to try to find its way to the tail
                    if (tailPath)
                    {
                        PathfindSnake();
                    }
                    else
                    {
                        // try to find the first walkable tile if there is no path to the fruit or tail
                        snakeDirection = Direction.up;  // firt we set the move direction to up
                        Tile[,] grid = GameManager.instance.grid;   // get a grid to compare if the tiles are walkable or not
                        //check if the direction is walkable else it will try the next direction
                        for (int i = 0; i < 3; i++)
                        {
                            if (grid[(int)GetNewHeadPosition().x, (int)GetNewHeadPosition().y].isWalkable)
                            {
                                // if the direction is walkable, there is no need to check an other one just break out
                                break;
                            }
                            else
                            {
                                //just increment the direction by 1 (available directions are 0, 1, 2, 3)
                                snakeDirection++;
                            }
                        }
                    }
                }
            }
            MoveSnake();    // move the snake
            GrowSnake();    // after the snake moved try to grow it
            RotateSnake(snakeDirection);    // set the rotation of the snake head. Mainly for the graphic of the head
        }

        gameTick += Time.fixedDeltaTime;    // increase the tick timer to manipulate the game speed/ movement speed
    }

    private Vector3 GetNewHeadPosition()
    {
        //set the movement of the snake, whit the use of snake direction
        switch (snakeDirection)
        {
            case Direction.up:
                return transform.position + Vector3.up;
            case Direction.down:
                return transform.position + Vector3.down;
            case Direction.left:
                return transform.position + Vector3.left;
            case Direction.right:
                return transform.position + Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    void OnDrawGizmos()
    {
        // draw what tiles are walkable or not in the editor
        foreach (var tile in GameManager.instance.grid)
        {
            if (!tile.isWalkable)
            { Gizmos.color = Color.red; }
            else { Gizmos.color = Color.green; }

            Gizmos.DrawCube(new Vector2(tile.x, tile.y), Vector3.one);
        }

        // used in the editor to draw the path the snake is taking
        if (pathfinding?.path != null)
        {
            foreach (var tile in pathfinding.path)
            {
                //Gizmos.color = Color.black;
                if (tile.isWalkable)
                { Gizmos.color = Color.white; }
                Gizmos.DrawCube(new Vector2(tile.x, tile.y), Vector3.one);
            }
        }
    }

    private void PathfindSnake()
    {
        // set the game speed when 10 fruits have been collected
        if (GameManager.instance.snakeSpeed >= gameSpeedMultiplier + 1) { gameSpeedMultiplier++; }
        
        // Check to see if the pathfinding path contains any elements
        if (pathfinding.path.Count > 0)
        {
            // take the first element of the path list.
            Tile newTile = pathfinding.path[0];
            // get the new direction, and then check what direction the snake should take depending on the value
            Vector3 getDirection = transform.position - new Vector3(newTile.x, newTile.y);
            if (getDirection.y == -1 && snakeDirection != Direction.down) { snakeDirection = Direction.up; }
            if (getDirection.y == 1 && snakeDirection != Direction.up) { snakeDirection = Direction.down; }
            if (getDirection.x == -1 && snakeDirection != Direction.left) { snakeDirection = Direction.right; }
            if (getDirection.x == 1 && snakeDirection != Direction.right) { snakeDirection = Direction.left; }
        }
    }

    private void SetSnakeMoveDirection()
    {
        // set the game speed when 10 fruits have been collected
        if (GameManager.instance.snakeSpeed >= gameSpeedMultiplier + 1) { gameSpeedMultiplier++; }

        // input to change the direction of the snake, but also make sure you cant go the opposite direction
        if (Input.GetKeyDown(KeyCode.UpArrow) && snakeDirection != Direction.down) { snakeDirection = Direction.up; }
        if (Input.GetKeyDown(KeyCode.DownArrow) && snakeDirection != Direction.up) { snakeDirection = Direction.down; }
        if (Input.GetKeyDown(KeyCode.LeftArrow) && snakeDirection != Direction.right) { snakeDirection = Direction.left; }
        if (Input.GetKeyDown(KeyCode.RightArrow) && snakeDirection != Direction.left) { snakeDirection = Direction.right; }
    }

    private void MoveSnake()
    {
        snakeMovement = GetNewHeadPosition();   // get the position the snake should move to
        snakeLastPosition = transform.position;  // store the last position of the snake head
        transform.position = snakeMovement;      // move the snake head to its new position

        // set the tile the snake head moved to a non walkable tile
        GameManager.instance.grid[(int)transform.position.x, (int)transform.position.y].isWalkable = false;

        if (linkedList.Count() > 0)
        {
            // taking care of the snake trail movement
            GameObject temp = linkedList.GetLast(); // get the last element in the linked list in a temp gameobject
            GameManager.instance.grid[(int)temp.transform.position.x, (int)temp.transform.position.y].isWalkable = true;    // set the tile to a walkable tile
            linkedList.Remove(linkedList.GetLast());    // remove the last element in the list
            temp.transform.position = snakeLastPosition;    // set the position of the temp gameobject to the snake heads last position
            linkedList.AddFirst(temp);  // put it back in the linked list as the first element in the linked list, behind the snake head. 
            //(technically the snake head is not in the list, but it contains the linked list. and just store the snake trail/body)
        }
    }

    private void GrowSnake()
    {
        // check if the snake should grow its length
        if (growSnake > 0)
        {
            growSnake--; // decrease the grow variable by 1 to make sure it wont spawn more elements than its suposed to
            GameObject body = Instantiate(GameManager.instance.bodyPrefab, snakeLastPosition, Quaternion.identity);  // create the gameobject in the game scene
            body.name = "body"; // just rename it, to make it look clean in the hierarcy
            linkedList.AddFirst(body);  // add the gameobject to the first element in the linked list,
        }
    }

    private void RotateSnake(Direction direction)
    {
        // set the rotation of the snake, mainly for the graphic to have right orientation
        transform.rotation = Quaternion.Euler(0, 0, (float)direction * 90);
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
