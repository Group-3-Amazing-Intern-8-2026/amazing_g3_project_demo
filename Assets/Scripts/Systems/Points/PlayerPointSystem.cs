using System;
using UnityEngine;

public class PlayerPointSystem : MonoBehaviour
{
    [Header("Current State")]
    [SerializeField] private int totalPoints = 0;

    public int TotalPoints => totalPoints;

    public event Action<int> OnPointsChanged;

    private void Start()
    {
        if (SnakeManager.Instance != null)
        {
            SnakeManager.Instance.OnFoodEaten += HandleFoodEaten;
        }
    }

    private void OnDestroy()
    {
        if (SnakeManager.Instance != null)
        {
            SnakeManager.Instance.OnFoodEaten -= HandleFoodEaten;
        }
    }

    private void HandleFoodEaten(FoodData foodData)
    {
        AddPoints(foodData.pointValue);
    }

    public void AddPoints(int amount)
    {
        totalPoints += amount;
        OnPointsChanged?.Invoke(totalPoints);
    }

    public void ResetPoints()
    {
        totalPoints = 0;
        OnPointsChanged?.Invoke(totalPoints);
    }
}