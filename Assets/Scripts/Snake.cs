using UnityEngine;

public class Snake : MonoBehaviour
{
    Vector3 movement;
    public float moveSpeed;

    public enum Direction { up, left, down, right }
    Direction snakeDirection;
    public bool hitWall;
    private void Awake()
    {
        Camera camera = Camera.main;
        transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, transform.position.z);
        movement = transform.position + Vector3.up * (0.01f * moveSpeed);
        gameObject.name = "Snake";
        if (moveSpeed <= 0) { moveSpeed = 10f; }
    }
    private void Update()
    {
        if (hitWall) { return; }
        if (Input.GetKeyDown(KeyCode.UpArrow)) { RotateSnake(Direction.up); }
        if (Input.GetKeyDown(KeyCode.DownArrow)) { RotateSnake(Direction.down); }
        if (Input.GetKeyDown(KeyCode.LeftArrow)) { RotateSnake(Direction.left); }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { RotateSnake(Direction.right); }

        switch (snakeDirection)
        {
            case Direction.up:
                movement = transform.position + Vector3.up * (0.01f * moveSpeed);
                break;
            case Direction.down:
                movement = transform.position + Vector3.down * (0.01f * moveSpeed);
                break;
            case Direction.left:
                movement = transform.position + Vector3.left * (0.01f * moveSpeed);
                break;
            case Direction.right:
                movement = transform.position + Vector3.right * (0.01f * moveSpeed);
                break;
            default:
                break;
        }

        transform.position = movement;
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
        if (collision.tag == "Wall" || collision.tag == "Body")
        {
            hitWall = true;
        }
    }
}
