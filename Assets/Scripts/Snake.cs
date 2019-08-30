using UnityEngine;

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
    private void Awake()
    {
        Camera camera = Camera.main;
        transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z);
        movement = transform.position + Vector3.up * (0.01f * moveSpeed);
        gameObject.name = "Snake";
        moveSpeed = GameManager.instance.snakeSpeed;
    }
    private void Update()
    {
        if (hitWall) { return; }
        if (GameManager.instance.snakeSpeed >= moveSpeed + 1)
        {
            moveSpeed++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { RotateSnake(Direction.up); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { RotateSnake(Direction.down); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { RotateSnake(Direction.left); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { RotateSnake(Direction.right); }

        
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

        if (tick > 1 / moveSpeed)
        {
            lastPosition = transform.position;
            transform.position = movement;
            GameObject head = GameManager.instance.linkedList.getFirst();
            GameObject last = GameManager.instance.linkedList.getLast();
            if (GameManager.instance.linkedList.count() > 1)
            {
                last.transform.position = lastPosition;
                GameManager.instance.linkedList.insertAfter(head, last);
                GameManager.instance.linkedList.remove(last);
            }
            
            tick = 0;
            if (grow > 0)
            {
                grow--;
                
                GameObject body = Instantiate(GameManager.instance.bodyPrefab, GameManager.instance.linkedList.getLast().transform.position, Quaternion.identity);
                GameManager.instance.linkedList.addLast(body);
            }
        }
        
        tick += Time.fixedDeltaTime;
    }
    private void FixedUpdate()
    {
        if (hitWall) { return; }
        

    }

    private void RotateSnake(Direction direction)
    {
        snakeDirection = direction;
        transform.rotation = Quaternion.Euler(0, 0, (float)direction * 90f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Wall" /*|| collision.tag == "Body"*/)
        {
            hitWall = true;
        }
        if (collision.tag == "Fruit")
        {
            grow++;
        }
    }
}
