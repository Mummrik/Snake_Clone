using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.SpawnFruit();
        GameManager.instance.snakeSpeed += 0.1f;
    }
}
