namespace Core.Enums
{
    public enum FoodType
    {
        Small,
        Medium,
        Big
    }

    public enum PowerType
    {
        None,
        DefensiveThorn,
        DefensiveRegen,
        OffensiveBulletEmitter,
        OffensiveProjectileShooter
    }

    public enum DefensiveTargetType
    {
        AllSegments,       // Áp dụng cho TOÀN BỘ bộ phận
        SpecificSegment,   // Chỉ áp dụng cho MỘT bộ phận cụ thể
        GlobalOnly         // KHÔNG áp dụng lên bộ phận, chỉ tăng chỉ số tổng
    }

    public enum SegmentPart
    {
        Head, 
        body,
    }
}
