using System;
using UnityEngine;

public class SnakeEater : MonoBehaviour
{
    [SerializeField] private float eatRadius = 0.6f;
    [SerializeField] private LayerMask foodLayer;

    public event Action<Food> OnFoodEaten;

    void Update()
    {
        CheckFoodOverlap();
    }

    void CheckFoodOverlap()
    {
        Collider2D[] hitFoods = Physics2D.OverlapCircleAll(transform.position, eatRadius, foodLayer);

        for (int i = 0; i < hitFoods.Length; i++)
        {
            Food food = hitFoods[i].GetComponent<Food>();
            if (food != null)
            {
                food.OnEaten();
                OnFoodEaten?.Invoke(food);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatRadius);
    }
}