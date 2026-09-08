using UnityEngine;
using System;
using System.Collections.Generic;

namespace Systems.Powers.Offensive
{
    public class OffensivePowerManager : MonoBehaviour
    {
        [Serializable]
        public class PowerSlot
        {
            [SerializeField] private int slotIndex;
            [SerializeField] private BodySegment assignedSegment;
            [SerializeField] private bool isUnlocked;
            [SerializeField] private bool isOccupied;

            public int SlotIndex => slotIndex;
            public BodySegment AssignedSegment => assignedSegment;
            public bool IsUnlocked => isUnlocked;
            public bool IsOccupied => isOccupied;

            public PowerSlot(int index)
            {
                slotIndex = index;
                assignedSegment = null;
                isUnlocked = false;
                isOccupied = false;
            }

            public void Unlock()
            {
                isUnlocked = true;
            }

            public void AssignSegment(BodySegment segment)
            {
                assignedSegment = segment;
            }

            public void ClearSegment()
            {
                assignedSegment = null;
                isOccupied = false;
            }

            public void SetOccupied(bool occupied)
            {
                isOccupied = occupied;
            }
        }

        [Header("Milestone Settings")]
        [Tooltip("Các mốc độ dài để mở khóa Power Slot.")]
        [SerializeField]
        private List<int> lengthMilestones = new List<int>();

        [Header("Slot Configuration")]
        [Tooltip("Số lượng Power Slot tối đa.")]
        [SerializeField]
        private int maxPerkSlots;

        private int currentLength;
        private int unlockedSlotsCount;

        private readonly List<BodySegment> activeSegments =
            new List<BodySegment>();

        private readonly List<PowerSlot> powerSlots =
            new List<PowerSlot>();

        public event Action<int> OnSlotUnlocked;

        public int CurrentLength => currentLength;
        public int UnlockedSlotsCount => unlockedSlotsCount;
        public IReadOnlyList<PowerSlot> PowerSlots => powerSlots;
        public IReadOnlyList<BodySegment> ActiveSegments => activeSegments;

        private void Start()
        {
            InitializeManager();
        }

        private void InitializeManager()
        {
            currentLength = 0;
            unlockedSlotsCount = 0;

            activeSegments.Clear();
            powerSlots.Clear();

            if (lengthMilestones == null)
            {
                lengthMilestones = new List<int>();
            }

            InitializeSlots();
        }

        private void InitializeSlots()
        {
            int slotCount = Mathf.Min(
                maxPerkSlots,
                lengthMilestones.Count
            );

            for (int i = 0; i < slotCount; i++)
            {
                powerSlots.Add(new PowerSlot(i));
            }
        }

        public void RegisterNewSegment(BodySegment newSegment)
        {
            if (newSegment == null)
                return;

            if (activeSegments.Contains(newSegment))
                return;

            activeSegments.Add(newSegment);

            UpdateLength(activeSegments.Count);

            AssignSegmentToAvailableSlot(newSegment);
        }

        public void UnregisterSegment(BodySegment segment)
        {
            if (segment == null)
                return;

            if (!activeSegments.Contains(segment))
                return;

            PowerSlot slot = GetSlotBySegment(segment);

            if (slot != null)
            {
                slot.ClearSegment();
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
            if (lengthMilestones == null)
                return;

            while (
                unlockedSlotsCount < lengthMilestones.Count &&
                unlockedSlotsCount < maxPerkSlots &&
                currentLength >= lengthMilestones[unlockedSlotsCount]
            )
            {
                UnlockNextSlot();
            }
        }

        private void UnlockNextSlot()
        {
            if (unlockedSlotsCount >= powerSlots.Count)
                return;

            PowerSlot slot = powerSlots[unlockedSlotsCount];

            slot.Unlock();

            unlockedSlotsCount++;

            OnSlotUnlocked?.Invoke(slot.SlotIndex);

            TryAssignSlotToSegment(slot);
        }

        private void AssignSegmentToAvailableSlot(BodySegment segment)
        {
            if (segment == null)
                return;

            for (int i = 0; i < powerSlots.Count; i++)
            {
                PowerSlot slot = powerSlots[i];

                if (!slot.IsUnlocked)
                    continue;

                if (slot.AssignedSegment != null)
                    continue;

                slot.AssignSegment(segment);
                return;
            }
        }

        private void TryAssignSlotToSegment(PowerSlot slot)
        {
            if (slot == null)
                return;

            if (!slot.IsUnlocked)
                return;

            if (slot.AssignedSegment != null)
                return;

            for (int i = 0; i < activeSegments.Count; i++)
            {
                BodySegment segment = activeSegments[i];

                if (GetSlotBySegment(segment) == null)
                {
                    slot.AssignSegment(segment);
                    return;
                }
            }
        }

        public PowerSlot GetSlot(int slotIndex)
        {
            if (slotIndex < 0 || slotIndex >= powerSlots.Count)
                return null;

            return powerSlots[slotIndex];
        }

        public PowerSlot GetSlotBySegment(BodySegment segment)
        {
            if (segment == null)
                return null;

            for (int i = 0; i < powerSlots.Count; i++)
            {
                if (powerSlots[i].AssignedSegment == segment)
                    return powerSlots[i];
            }

            return null;
        }

        public BodySegment GetSegmentBySlot(int slotIndex)
        {
            PowerSlot slot = GetSlot(slotIndex);

            if (slot == null)
                return null;

            return slot.AssignedSegment;
        }

        public bool HasAvailableSlot()
        {
            for (int i = 0; i < powerSlots.Count; i++)
            {
                PowerSlot slot = powerSlots[i];

                if (slot.IsUnlocked && !slot.IsOccupied)
                    return true;
            }

            return false;
        }

        public bool IsSlotUnlocked(int slotIndex)
        {
            PowerSlot slot = GetSlot(slotIndex);

            return slot != null && slot.IsUnlocked;
        }

        public bool IsSlotOccupied(int slotIndex)
        {
            PowerSlot slot = GetSlot(slotIndex);

            return slot != null && slot.IsOccupied;
        }

        public bool SetSlotOccupied(int slotIndex, bool occupied)
        {
            PowerSlot slot = GetSlot(slotIndex);

            if (slot == null)
                return false;

            if (!slot.IsUnlocked)
                return false;

            slot.SetOccupied(occupied);

            return true;
        }
    }
}