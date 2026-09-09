using System;
using UnityEngine;

public class SnakeEater : MonoBehaviour
{
    [SerializeField] private float eatRadius = 0.6f;
    [SerializeField] private LayerMask foodLayer;

    public event Action<FoodData> OnFoodConsumed;

    private void Update()
    {
        CheckFoodOverlap();
    }

    private void CheckFoodOverlap()
    {
        Collider2D[] hitFoods = Physics2D.OverlapCircleAll(transform.position, eatRadius, foodLayer);

        for (int i = 0; i < hitFoods.Length; i++)
        {
            Food food = hitFoods[i].GetComponent<Food>();
            if (food != null)
            {
                FoodData data = food.Collect();
                if (data != null)
                {
                    OnFoodConsumed?.Invoke(data);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatRadius);
    }
}