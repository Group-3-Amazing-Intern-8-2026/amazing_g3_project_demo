using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class Food : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;

    public FoodData Data { get; private set; }

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Initialize(FoodData data)
    {
        Data = data;
        ApplyVisual();
    }

    private void ApplyVisual()
    {
        if (Data == null || spriteRenderer == null) return;

        if (Data.sprite != null)
        {
            spriteRenderer.sprite = Data.sprite;
        }

        spriteRenderer.color = Data.foodColor;
        transform.localScale = Vector3.one * Data.size;
    }
    public FoodData Collect()
    {
        FoodData collectedData = Data;
        FoodSpawner.Instance?.RespawnFood(this);
        return collectedData;
    }
}