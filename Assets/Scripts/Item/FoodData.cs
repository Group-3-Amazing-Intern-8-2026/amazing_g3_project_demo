using UnityEngine;

public enum FoodType
{
    Small,
    Medium,
    Big
}

[CreateAssetMenu(fileName = "NewFoodData", menuName = "Item/FoodData", order = 1)]
public class FoodData : ScriptableObject
{
    [Header("Phân loại")]
    public FoodType foodType;

    [Header("Điểm số")]
    [Tooltip("Số điểm cộng khi player ăn loại thức ăn này. Small=10, Medium=30, Big=60")]
    public int pointValue = 10;

    [Header("Tiến trình phát triển")]
    [Range(0f, 100f)]
    [Tooltip("Phần trăm đóng góp vào việc hoàn thành 1 đốt thân (segment). Small=10%, Medium=30%, Big=60%")]
    public float segmentProgressPercent = 10f;

    [Header("Hiển thị")]
    public Color foodColor = Color.white;

    [Tooltip("Tùy chọn: prefab riêng nếu loại thức ăn này cần hình dạng/mesh khác biệt")]
    public GameObject foodPrefab;
}