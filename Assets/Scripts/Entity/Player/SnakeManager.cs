using UnityEngine;

[RequireComponent(typeof(SnakeMovement))]
[RequireComponent(typeof(SnakeBodyManager))]
[RequireComponent(typeof(SnakeEater))]
public class SnakeManager : MonoBehaviour
{
    private SnakeMovement movement;
    private SnakeBodyManager bodyManager;
    private SnakeEater eater;

    public SnakeMovement Movement => movement;
    public SnakeBodyManager BodyManager => bodyManager;
    public SnakeEater Eater => eater;

    void Awake()
    {
        movement = GetComponent<SnakeMovement>();
        bodyManager = GetComponent<SnakeBodyManager>();
        eater = GetComponent<SnakeEater>();
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
