using UnityEngine;

public enum FoodType
{
    Small,
    Medium,
    Big
}

[CreateAssetMenu(fileName = "NewFoodData", menuName = "Item/FoodData", order = 1)]
public class FoodData : ScriptableObject
{
    public FoodType foodType;
    public int pointValue = 10;
    
    [Range(0f, 100f)]
    public float segmentProgressPercent = 10f;

    [Header("Spawn Settings")]
    //Higher number = more common | Lower number = rarer"
    [Min(0f)] public float spawnWeight = 10f;

    [Header("Visual Settings")]
    public Sprite sprite;
    public Color foodColor = Color.white;
    public float size = 1f;
}