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
        AllSegments,
        SpecificSegment,   
        GlobalOnly         
    }

    public enum SegmentPart
    {
        Head, 
        body,
    }
}
