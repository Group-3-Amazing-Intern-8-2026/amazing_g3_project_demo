namespace Core.Interfaces
{
    public interface IDamageable
    {
        float CurrentHealth { get; }
        float MaxHealth { get; }
        void TakeDamage(float amount);
        void Heal(float amount);
    }
}
