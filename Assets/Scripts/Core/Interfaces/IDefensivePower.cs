namespace Core.Interfaces
{
    public interface IDefensivePower
    {
        void ApplyDefensiveEffect(BodySegment segment);
        void RemoveDefensiveEffect(BodySegment segment);
    }
    public static class DefensivePowerExtensions
    {
        public static void ApplyToSegments(this IDefensivePower power, System.Collections.Generic.List<BodySegment> segments)
        {
            if (segments == null) return;
            for (int i = 0; i < segments.Count; i++)
            {
                power.ApplyDefensiveEffect(segments[i]);
            }
        }

        public static void RemoveFromSegments(this IDefensivePower power, System.Collections.Generic.List<BodySegment> segments)
        {
            if (segments == null) return;
            for (int i = 0; i < segments.Count; i++)
            {
                power.RemoveDefensiveEffect(segments[i]);
            }
        }
    }
}
