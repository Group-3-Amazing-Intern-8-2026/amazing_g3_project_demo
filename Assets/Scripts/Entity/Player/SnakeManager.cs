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

    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("[SnakeManager] Đã có 1 Instance tồn tại, huỷ bản sao thừa.");
            Destroy(gameObject);
        }
    }

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

    /// LƯU Ý: Không gọi Grow() ở đây. PlayerPointSystem.AddFood() đã tự quyết định, khi nào Grow() dựa trên % tiến 
    /// trình đạt 100% (có carry-over phần dư).
    /// Nếu gọi Grow() ở cả đây lẫn PlayerPointSystem, rắn sẽ dài ra 2 lần cho 1 lần ăn.
    /// Giữ sự kiện này để gắn SFX/VFX "ăn" nếu cần sau này.
    private void HandleFoodEaten(Food food)
    {
        
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