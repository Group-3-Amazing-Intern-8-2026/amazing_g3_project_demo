using UnityEngine;

public class FoodSpawner : ObjectPoolBase<Food>
{
    public static FoodSpawner Instance { get; private set; }

    [SerializeField] private int initialFoodCount = 100;
    [SerializeField] private float arenaSize = 30f;
    [SerializeField] private FoodData[] foodDataOptions;

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

    private void Start()
    {
        SpawnFoodBatch(initialFoodCount);
    }

    protected override void OnGetItem(Food food)
    {
        base.OnGetItem(food);

        food.transform.position = new Vector2(
            Random.Range(-arenaSize, arenaSize),
            Random.Range(-arenaSize, arenaSize)
        );

        FoodData chosen = ChooseWeightedFoodData();
        if (chosen != null)
        {
            food.Initialize(chosen);
        }
    }

    private FoodData ChooseWeightedFoodData()
    {
        float totalWeight = 0f;
        foreach (var data in foodDataOptions)
        {
            if (data != null) totalWeight += data.spawnWeight;
        }

        if (totalWeight <= 0f) return foodDataOptions[0];

        float roll = Random.Range(0f, totalWeight);
        float cumulative = 0f;

        foreach (var data in foodDataOptions)
        {
            if (data == null) continue;

            cumulative += data.spawnWeight;
            if (roll <= cumulative) return data;
        }

        return foodDataOptions[foodDataOptions.Length - 1];
    }

    public void SpawnFoodBatch(int count) => GetList(count);

    public void RespawnFood(Food food)
    {
        Release(food);
        Get();
    }
}