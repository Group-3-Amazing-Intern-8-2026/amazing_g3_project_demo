namespace Core.Interfaces
{
    public interface IOffensivePower
    {
        void ApplyOffensiveEffect(BodySegment segment);
        void RemoveOffensiveEffect(BodySegment segment);
    }
}
