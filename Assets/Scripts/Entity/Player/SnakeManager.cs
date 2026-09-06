using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] private SnakeMovement movement;
    [SerializeField] private SnakeBodyManager bodyManager;
    [SerializeField] private SnakeEater eater;

    public SnakeMovement Movement => movement;
    public SnakeBodyManager BodyManager => bodyManager;
    public SnakeEater Eater => eater;

    void OnEnable()
    {
        if (eater != null)
        {
            eater.OnFoodEaten += HandleFoodEaten;
        }
    }

    void OnDisable()
    {
        if (eater != null)
        {
            eater.OnFoodEaten -= HandleFoodEaten;
        }
    }

    private void HandleFoodEaten(Food food)
    {
        Grow();
    }

    public void Grow()
    {
        if (bodyManager != null)
        {
            bodyManager.Grow();
        }
    }

    public void Shrink()
    {
        if (bodyManager != null)
        {
            bodyManager.Shrink();
        }
    }
}