using UnityEngine;
using UnityEngine.UI;

public class PointUI : MonoBehaviour
{
    [Header("Tham chiếu hệ thống")]
    [Tooltip("Nếu để trống, sẽ tự tìm PlayerPointSystem trong scene lúc OnEnable")]
    [SerializeField] private PlayerPointSystem pointSystem;

    [Header("UI Elements")]
    [SerializeField] private Text pointText;        // Đổi sang TMP_Text nếu project dùng TextMeshPro
    [SerializeField] private Slider progressSlider;  // Giá trị chuẩn hóa 0 - 1

    private void OnEnable()
    {
        if (pointSystem == null)
        {
            pointSystem = FindFirstObjectByType<PlayerPointSystem>();
        }

        if (pointSystem == null)
        {
            Debug.LogWarning("[PointUI] Không tìm thấy PlayerPointSystem trong scene.");
            return;
        }

        pointSystem.OnPointsChanged += HandlePointsChanged;
        pointSystem.OnSegmentProgressUpdated += HandleProgressUpdated;

        // Đồng bộ UI với giá trị hiện tại ngay khi bật lên
        HandlePointsChanged(pointSystem.TotalPoints);
        HandleProgressUpdated(pointSystem.SegmentProgress);
    }

    private void OnDisable()
    {
        if (pointSystem == null) return;

        pointSystem.OnPointsChanged -= HandlePointsChanged;
        pointSystem.OnSegmentProgressUpdated -= HandleProgressUpdated;
    }

    private void HandlePointsChanged(int newTotal)
    {
        if (pointText != null)
        {
            pointText.text = $"Điểm: {newTotal}";
        }
    }

    private void HandleProgressUpdated(float progressPercent)
    {
        if (progressSlider != null)
        {
            progressSlider.value = progressPercent / 100f; // Slider chuẩn 0-1
        }
    }
}