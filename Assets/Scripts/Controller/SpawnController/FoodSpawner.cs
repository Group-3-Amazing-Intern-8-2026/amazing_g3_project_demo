using UnityEngine;

public class FoodSpawner : ObjectPoolBase<Food>
{
    public static FoodSpawner Instance;

    [SerializeField] private int initialFoodCount = 100;
    [SerializeField] private float arenaSize = 30f;

    protected override void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        initialCapacity = initialFoodCount;
        maxSize = 200;
        base.Awake();
    }

    void Start()
    {
        for (int i = 0; i < initialFoodCount; i++)
        {
            SpawnFoodAtRandomPosition();
        }
    }

    protected override Food CreateItem()
    {
        Food food;
        if (prefab != null)
        {
            food = Instantiate(prefab, transform);
        }
        else
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            sphere.transform.SetParent(transform);

            Collider col = sphere.GetComponent<Collider>();
            if (col != null) col.isTrigger = true;
            food = sphere.AddComponent<Food>();
        }
        return food;
    }

    protected override void OnGetItem(Food food)
    {
        base.OnGetItem(food);
        Vector2 randomPos = new Vector2(
            Random.Range(-arenaSize, arenaSize),
            Random.Range(-arenaSize, arenaSize)
        );
        food.transform.position = randomPos;
    }

    void SpawnFoodAtRandomPosition()
    {
        Get();
    }

    public void RespawnFood(GameObject foodObj)
    {
        Food food = foodObj.GetComponent<Food>();
        if (food != null)
        {
            Release(food);
            Get();
        }
    }
}
