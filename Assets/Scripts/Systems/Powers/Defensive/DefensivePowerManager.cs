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
        [SerializeField] private List<int> lengthMilestones; 
        
        [Header("Slot Configuration")]
        [SerializeField] private int maxPerkSlots;

        private int currentLength;
        private int unlockedSlotsCount;
        private List<IDefensivePower> equippedDefensivePowers = new List<IDefensivePower>();
        private List<BodySegment> activeSegments = new List<BodySegment>();

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
            
            // Đảm bảo list không bị null khi chạy logic
            if (lengthMilestones == null) lengthMilestones = new List<int>();
        }

        public void RegisterNewSegment(BodySegment newSegment)
        {
            if (newSegment == null) return;
            
            activeSegments.Add(newSegment);

            foreach (var power in equippedDefensivePowers)
            {
                power.ApplyDefensiveEffect(newSegment);
            }

            UpdateLength(activeSegments.Count);
        }

        public void UnregisterSegment(BodySegment segment)
        {
            if (segment == null || !activeSegments.Contains(segment)) return;

            foreach (var power in equippedDefensivePowers)
            {
                power.RemoveDefensiveEffect(segment);
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

            for (int i = unlockedSlotsCount; i < lengthMilestones.Count; i++)
            {
                if (currentLength >= lengthMilestones[i] && unlockedSlotsCount < maxPerkSlots)
                {
                    unlockedSlotsCount++;
                    OnSlotUnlocked?.Invoke(unlockedSlotsCount);
                }
            }
        }

        public bool EquipPower(IDefensivePower power)
        {
            if (equippedDefensivePowers.Count >= unlockedSlotsCount) return false;

            if (!equippedDefensivePowers.Contains(power))
            {
                equippedDefensivePowers.Add(power);
                power.ApplyDefensiveEffect();
                
                foreach (var segment in activeSegments)
                {
                    power.ApplyDefensiveEffect(segment);
                }

                OnPowerEquipped?.Invoke(power);
                return true;
            }
            return false;
        }

        public bool UnequipPower(IDefensivePower power)
        {
            if (equippedDefensivePowers.Contains(power))
            {
                power.RemoveDefensiveEffect();
                foreach (var segment in activeSegments)
                {
                    power.RemoveDefensiveEffect(segment);
                }

                equippedDefensivePowers.Remove(power);
                return true;
            }
            return false;
        }
    }
}