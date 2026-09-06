using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static FoodSpawner Instance;

    public GameObject foodPrefab;
    public int initialFoodCount = 100;
    public float arenaSize = 30f;

    private List<GameObject> foodPool = new List<GameObject>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        for (int i = 0; i < initialFoodCount; i++)
        {
            SpawnFoodAtRandomPosition();
        }
    }

    void SpawnFoodAtRandomPosition()
    {
        Vector2 randomPos = new Vector2(
            Random.Range(-arenaSize, arenaSize),
            Random.Range(-arenaSize, arenaSize)
        );

        GameObject food;
        if (foodPrefab != null)
        {
            food = Instantiate(foodPrefab, randomPos, Quaternion.identity);
        }
        else
        {
            food = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            food.transform.position = randomPos;
            food.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            Collider col = food.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
            food.AddComponent<Food>();
        }

        foodPool.Add(food);
    }

    public void RespawnFood(GameObject food)
    {
        Vector2 randomPos = new Vector2(
            Random.Range(-arenaSize, arenaSize),
            Random.Range(-arenaSize, arenaSize)
        );
        food.transform.position = randomPos;
    }
}
