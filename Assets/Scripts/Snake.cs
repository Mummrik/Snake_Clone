using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    Vector3 movement;
    //LinkedList<GameObject> linkedList;
    public float moveSpeed;

    public enum Direction { up, left, down, right }
    public Direction snakeDirection;
    public bool hitWall;

    public float tick;
    int grow;
    Vector3 lastPosition;
    LinkedList<GameObject> linkedList;
    private void Awake()
    {
        Camera camera = Camera.main;
        transform.position = new Vector3(camera.transform.position.x - 0.5f, camera.transform.position.y - 0.5f, transform.position.z);
        movement = transform.position + Vector3.up * (0.01f * moveSpeed);
        gameObject.name = "Snake";
        moveSpeed = GameManager.instance.snakeSpeed;
        linkedList = new LinkedList<GameObject>();
        grow = 3;
    }
    private void Update()
    {
        //if (hitWall) { return; }
        if (tick > 1 / moveSpeed)
        {
            if (hitWall)
            {
                SceneManager.LoadScene(0);
            }
            else
            {
                lastPosition = transform.position;
                transform.position = movement;

                if (linkedList.count() > 0)
                {
                    GameObject temp = linkedList.getLast();
                    linkedList.remove(linkedList.getLast());
                    temp.transform.position = lastPosition;
                    linkedList.addFirst(temp);

                }

                tick = 0;
                if (grow > 0)
                {
                    grow--;
                    GameObject body = Instantiate(GameManager.instance.bodyPrefab, transform.position, Quaternion.identity);
                    linkedList.addLast(body);
                    // add new body to the last element
                }
                RotateSnake(snakeDirection);
            }

        }

        if (GameManager.instance.snakeSpeed >= moveSpeed + 1)
        {
            moveSpeed++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { snakeDirection = Direction.up; }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { snakeDirection = Direction.down; }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { snakeDirection = Direction.left; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { snakeDirection = Direction.right; }


        switch (snakeDirection)
        {
            case Direction.up:
                movement = transform.position + Vector3.up;
                break;
            case Direction.down:
                movement = transform.position + Vector3.down;
                break;
            case Direction.left:
                movement = transform.position + Vector3.left;
                break;
            case Direction.right:
                movement = transform.position + Vector3.right;
                break;
            default:
                break;
        }

        tick += Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {



    }

    private void RotateSnake(Direction direction)
    {
        snakeDirection = direction;
        transform.rotation = Quaternion.Euler(0, 0, (float)direction * 90f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" || collision.tag == "Body")
        {
            hitWall = true;
        }
        if (collision.tag == "Fruit")
        {
            grow++;
        }
    }
}
