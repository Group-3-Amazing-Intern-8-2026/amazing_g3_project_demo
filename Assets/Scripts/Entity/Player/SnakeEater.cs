using UnityEngine;

[RequireComponent(typeof(SnakeBodyManager))]
public class SnakeEater : MonoBehaviour
{
    [Header("Eating Settings")]
    [SerializeField] private float eatRadius = 0.6f;
    [SerializeField] private LayerMask foodLayer;

    private SnakeBodyManager bodyManager;

    void Start()
    {
        bodyManager = GetComponent<SnakeBodyManager>();
    }

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
                bodyManager.Grow();
                food.OnEaten();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatRadius);
    }
}
