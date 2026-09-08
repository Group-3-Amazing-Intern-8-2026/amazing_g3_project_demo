using UnityEngine;
using Core.Interfaces;
using Core.Enums;

namespace Systems.Powers.Defensive
{
    [CreateAssetMenu(fileName = "NewDefensivePower", menuName = "Powers/Create New Defensive Power")]
    public class GenericDefensivePower : ScriptableObject, IDefensivePower
    {
        
        public string skillName = "New Defensive Skill";

        [Tooltip("Chọn cách skill này tác động lên các bộ phận")]
        [SerializeField] private DefensiveTargetType targetType;
        
        [Tooltip("Nếu chọn SpecificSegment, hãy chỉ định rõ bộ phận nào được nhận buff")]
        [SerializeField] Core.Enums.SegmentPart targetSegment;

        [SerializeField]  float bonusDefense;
        [SerializeField]  float bonusMaxHealth;
    
        [Tooltip("Hiệu ứng Particle sẽ sinh ra trên bộ phận khi có buff")]
        [SerializeField] private GameObject defenseVFXPrefab;

        private bool ShouldApplyTo(BodySegment segment)
        {
            if (segment == null) return false;

            switch (targetType)
            {
                case DefensiveTargetType.AllSegments:
                    return true;

                case DefensiveTargetType.SpecificSegment:
                    return segment.partType == targetSegment;

                case DefensiveTargetType.GlobalOnly:
                    return false;

                default:
                    return false;
            }
        }

        public void ApplyDefensiveEffect(BodySegment segment)
        {
            if (!ShouldApplyTo(segment)) return;

            if (defenseVFXPrefab != null)
            {
                GameObject vfx = Instantiate(defenseVFXPrefab, segment.transform.position, segment.transform.rotation);
                vfx.transform.SetParent(segment.transform);
                vfx.name = $"VFX_{skillName}";
            }

            Debug.Log($"[{skillName}] Đã áp dụng hiệu ứng lên: {segment.name}");
        }

        public void RemoveDefensiveEffect(BodySegment segment)
        {
            if (!ShouldApplyTo(segment)) return;

            Transform activeVFX = segment.transform.Find($"VFX_{skillName}");
            if (activeVFX != null)
            {
                Destroy(activeVFX.gameObject);
            }

            Debug.Log($"[{skillName}] Đã gỡ bỏ hiệu ứng khỏi: {segment.name}");
        }
    }
    
}
