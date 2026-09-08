using UnityEngine;
using System;
using System.Collections.Generic;
using Core.Interfaces;

namespace Systems.Powers.Defensive
{
    public class DefensivePowerManager : MonoBehaviour
    {
        [Header("Milestone Settings")]
        [Tooltip("Nhập các mốc độ dài để mở khóa slot (Ví dụ: Phần tử 0 = 10, Phần tử 1 = 25)")]
        [SerializeField] private List<int> lengthMilestones = new List<int>(); 
        
        [Header("Slot Configuration")]
        [SerializeField] private int maxPerkSlots;

        private int currentLength;
        private int unlockedSlotsCount;
        private readonly List<IDefensivePower> equippedDefensivePowers = new List<IDefensivePower>();
        private readonly List<BodySegment> activeSegments = new List<BodySegment>();

        public event Action<int> OnSlotUnlocked;
        public event Action<IDefensivePower> OnPowerEquipped;

        private void Start()
        {
            InitializeManager();
        }

        private void InitializeManager()
        {
            currentLength = 0;
            unlockedSlotsCount = 0;
            equippedDefensivePowers.Clear();
            activeSegments.Clear();
        }

        public void RegisterNewSegment(BodySegment newSegment)
        {
            if (newSegment == null) return;
            
            activeSegments.Add(newSegment);

            // Kích hoạt buff của TẤT CẢ kỹ năng đang trang bị lên bộ phận MỚI này
            for (int i = 0; i < equippedDefensivePowers.Count; i++)
            {
                equippedDefensivePowers[i].ApplyDefensiveEffect(newSegment);
            }

            UpdateLength(activeSegments.Count);
        }

        public void UnregisterSegment(BodySegment segment)
        {
            if (segment == null || !activeSegments.Contains(segment)) return;

            // Xóa buff của TẤT CẢ kỹ năng khỏi bộ phận bị hủy này
            for (int i = 0; i < equippedDefensivePowers.Count; i++)
            {
                equippedDefensivePowers[i].RemoveDefensiveEffect(segment);
            }

            activeSegments.Remove(segment);
            UpdateLength(activeSegments.Count);
        }

        private void UpdateLength(int newLength)
        {
            currentLength = newLength;
            CheckMilestones();
        }

        private void CheckMilestones()
        {
            if (lengthMilestones == null) return;

            // Kiểm tra tăng slot (Tiến)
            for (int i = unlockedSlotsCount; i < lengthMilestones.Count; i++)
            {
                if (currentLength >= lengthMilestones[i] && unlockedSlotsCount < maxPerkSlots)
                {
                    unlockedSlotsCount++;
                    OnSlotUnlocked?.Invoke(unlockedSlotsCount);
                }
                else break; // Nếu mốc hiện tại chưa đạt, các mốc sau cũng vậy
            }

            // Sửa lỗi: Nếu chiều dài giảm xuống thấp hơn mốc cũ, bạn có muốn khóa bớt slot lại không?
            // Nếu có logic khóa slot khi nhân vật ngắn đi, ta sẽ handle thêm ở đây.
        }

        public bool EquipPower(IDefensivePower power)
        {
            if (power == null || equippedDefensivePowers.Count >= unlockedSlotsCount) return false;

            if (!equippedDefensivePowers.Contains(power))
            {
                equippedDefensivePowers.Add(power);
                
                // Áp dụng kỹ năng lên TOÀN BỘ các bộ phận đang có (Dùng vòng lặp for tối ưu)
                for (int i = 0; i < activeSegments.Count; i++)
                {
                    power.ApplyDefensiveEffect(activeSegments[i]);
                }

                OnPowerEquipped?.Invoke(power);
                return true;
            }
            return false;
        }

        public bool UnequipPower(IDefensivePower power)
        {
            if (power == null || !equippedDefensivePowers.Contains(power)) return false;

            // Gỡ kỹ năng khỏi TOÀN BỘ các bộ phận
            for (int i = 0; i < activeSegments.Count; i++)
            {
                power.RemoveDefensiveEffect(activeSegments[i]);
            }

            equippedDefensivePowers.Remove(power);
            return true;
        }
    }
}