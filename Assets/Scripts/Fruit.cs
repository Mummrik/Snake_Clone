using UnityEngine;

public class Fruit : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.snakeSpeed += 0.1f;    // increase the speed of the snake (only each whole number will make a speed change though)
        GameManager.instance.appleCount++;  // used for keeping track of the score
        GameManager.instance.SpawnFruit();  // set the position of the fruit
    }
}
