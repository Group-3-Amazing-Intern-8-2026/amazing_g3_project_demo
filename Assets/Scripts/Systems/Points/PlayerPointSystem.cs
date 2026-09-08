using System;
using UnityEngine;

public class PlayerPointSystem : MonoBehaviour
{
    [Header("Trạng thái hiện tại")]
    [SerializeField] private int totalPoints = 0;
    [SerializeField] private float segmentProgress = 0f; // 0 - 100

    public int TotalPoints => totalPoints;
    public float SegmentProgress => segmentProgress;

    public event Action<int> OnPointsChanged;

    public event Action<float> OnSegmentProgressUpdated;

    public void AddFood(FoodData foodData)
    {
        if (foodData == null) return;

        // --- Cộng điểm ---
        totalPoints += foodData.pointValue;
        OnPointsChanged?.Invoke(totalPoints);

        // --- Cộng tiến trình, xử lý carry-over khi vượt 100% ---
        segmentProgress += foodData.segmentProgressPercent;

        while (segmentProgress >= 100f)
        {
            segmentProgress -= 100f; // Giữ lại phần dư cho đốt tiếp theo
            TriggerGrow();
        }

        OnSegmentProgressUpdated?.Invoke(segmentProgress);
    }

    private void TriggerGrow()
    {
        if (SnakeManager.Instance != null)
        {
            SnakeManager.Instance.Grow();
        }
        else
        {
            Debug.LogWarning("[PlayerPointSystem] SnakeManager.Instance là null — không thể Grow().");
        }
    }
    
    public void ResetProgress()
    {
        totalPoints = 0;
        segmentProgress = 0f;
        OnPointsChanged?.Invoke(totalPoints);
        OnSegmentProgressUpdated?.Invoke(segmentProgress);
    }
}