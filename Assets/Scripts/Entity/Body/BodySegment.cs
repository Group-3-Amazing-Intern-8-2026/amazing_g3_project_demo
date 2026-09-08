using UnityEngine;
using Core.Interfaces;
using Core.Enums;

public class BodySegment : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    private int segmentIndex;

    public int SegmentIndex => segmentIndex;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;

    public delegate void SegmentDeathHandler(BodySegment segment);
    public event SegmentDeathHandler OnSegmentDeath;
    [Header("Segment Identity")]
    [Tooltip("Chọn loại bộ phận này trong Inspector")]
    public SegmentPart partType;

    public void Initialize(int index, float health)
    {
        segmentIndex = index;
        maxHealth = health;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth -= amount;
        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }

    private void Die()
    {
        OnSegmentDeath?.Invoke(this);
    }
    
}
