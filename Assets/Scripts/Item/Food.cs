using UnityEngine;

public class Food : MonoBehaviour
{
    public int foodValue = 1;

    public void OnEaten()
    {
        FoodSpawner.Instance?.RespawnFood(gameObject);
    }
}