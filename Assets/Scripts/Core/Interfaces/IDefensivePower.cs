namespace Core.Interfaces
{
    public interface IDefensivePower
    {
        void ApplyDefensiveEffect(BodySegment segment);
        void RemoveDefensiveEffect(BodySegment segment);
        void ApplyDefensiveEffect();
        void RemoveDefensiveEffect();
    }
}
