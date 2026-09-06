using UnityEngine;

public class Food : MonoBehaviour
{
    public int growthAmount = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        SnakeHead snake = collision.GetComponent<SnakeHead>();
        if (snake != null)
        {
            snake.Grow();
            FoodSpawner.Instance?.RespawnFood(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        SnakeHead snake = other.GetComponent<SnakeHead>();
        if (snake != null)
        {
            snake.Grow();
            FoodSpawner.Instance?.RespawnFood(gameObject);
        }
    }
}
