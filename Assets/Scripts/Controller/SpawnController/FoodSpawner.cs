using UnityEngine;

public class FoodSpawner : ObjectPoolBase<Food>
{
    public static FoodSpawner Instance;

    [SerializeField] private int initialFoodCount = 100;
    [SerializeField] private float arenaSize = 30f;

    [Header("Mix Food theo trọng số")]
    [Tooltip("Gán 3 asset Small/Medium/Big (Create > Item > FoodData)")]
    [SerializeField] private FoodData[] foodDataOptions;
 
    [Tooltip("Trọng số tương ứng theo thứ tự trên.")]
    [SerializeField] private float[] spawnWeights;

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
         // Gán loại thức ăn (Small/Medium/Big) theo trọng số mỗi lần lấy từ pool.
        FoodData chosen = ChooseWeightedFoodData();
        if (chosen != null)
        {
            food.Initialize(chosen);
        }
    }

    private FoodData ChooseWeightedFoodData()
    {
        if (foodDataOptions == null || foodDataOptions.Length == 0)
        {
            Debug.LogWarning("[FoodSpawner] foodDataOptions rỗng — chưa gán Small/Medium/Big trong Inspector.");
            return null;
        }
 
        float total = 0f;
        foreach (var w in spawnWeights) total += w;
 
        if (total <= 0f) return foodDataOptions[0];
 
        float roll = Random.Range(0f, total);
        float cumulative = 0f;
 
        for (int i = 0; i < spawnWeights.Length && i < foodDataOptions.Length; i++)
        {
            cumulative += spawnWeights[i];
            if (roll <= cumulative) return foodDataOptions[i];
        }
 
        return foodDataOptions[foodDataOptions.Length - 1];
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
