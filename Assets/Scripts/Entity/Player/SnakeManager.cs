using System;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    public static SnakeManager Instance { get; private set; }

    [SerializeField] private SnakeMovement movement;
    [SerializeField] private SnakeBodyManager bodyManager;
    [SerializeField] private SnakeEater eater;

    public SnakeMovement Movement => movement;
    public SnakeBodyManager BodyManager => bodyManager;
    public SnakeEater Eater => eater;
    public event Action<FoodData> OnFoodEaten;

    private void Awake()
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

    private void OnEnable()
    {
        if (eater != null)
        {
            eater.OnFoodConsumed += HandleFoodConsumed;
        }
    }

    private void OnDisable()
    {
        if (eater != null)
        {
            eater.OnFoodConsumed -= HandleFoodConsumed;
        }
    }
    public void Grow() => bodyManager?.Grow();
    public void Shrink() => bodyManager?.Shrink();

    private void HandleFoodConsumed(FoodData foodData)
    {
        if (foodData == null) return;

        if (bodyManager != null)
        {
            bodyManager.AddGrowthProgress(foodData.segmentProgressPercent);
        }
        OnFoodEaten?.Invoke(foodData);
    }

}