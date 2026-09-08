using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Food : MonoBehaviour
{
    [SerializeField] private FoodData foodData;

    public FoodData Data => foodData;

    private SpriteRenderer spriteRenderer;
    private PlayerPointSystem cachedPointSystem;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplyVisual();
    }

    public void Initialize(FoodData data)
    {
        foodData = data;
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (foodData == null || spriteRenderer == null) return;
        spriteRenderer.color = foodData.foodColor;
    }

    public void OnEaten()
    {
        if (cachedPointSystem == null)
        {
            cachedPointSystem = FindFirstObjectByType<PlayerPointSystem>();
        }

        if (cachedPointSystem != null && foodData != null)
        {
            cachedPointSystem.AddFood(foodData);
        }
        else if (foodData == null)
        {
            Debug.LogWarning("[Food] OnEaten() nhưng foodData null — FoodSpawner chưa Initialize() đúng cách.");
        }

        FoodSpawner.Instance?.RespawnFood(gameObject);
    }
}