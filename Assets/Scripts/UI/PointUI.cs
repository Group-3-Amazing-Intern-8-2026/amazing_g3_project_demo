using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text pointText;
    [SerializeField] private Slider progressSlider;

    [SerializeField] private PlayerPointSystem pointSystem;
    private SnakeBodyManager bodyManager;

    private void Start()
    {
        if (bodyManager == null && SnakeManager.Instance != null) bodyManager = SnakeManager.Instance.BodyManager;

        if (pointSystem != null)
        {
            pointSystem.OnPointsChanged += UpdatePoints;
            UpdatePoints(pointSystem.TotalPoints);
        }

        if (bodyManager != null)
        {
            bodyManager.OnSegmentProgressUpdated += UpdateProgress;
            UpdateProgress(bodyManager.SegmentProgress);
        }
    }

    private void OnDestroy()
    {
        if (pointSystem != null) pointSystem.OnPointsChanged -= UpdatePoints;
        if (bodyManager != null) bodyManager.OnSegmentProgressUpdated -= UpdateProgress;
    }

    private void UpdatePoints(int totalPoints)
    {
        if (pointText != null) pointText.text = $"Point: {totalPoints}";
    }

    private void UpdateProgress(float progressPercent)
    {
        if (progressSlider != null) progressSlider.value = progressPercent / 100f;
    }
}