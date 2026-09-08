using System.Collections.Generic;
using UnityEngine;

public class OffensivePowerManager : MonoBehaviour
{
    [System.Serializable]
    public class LengthMilestone
    {
        [Min(0)]
        public int requiredLength;

        [Min(0)]
        public int unlockedSlots;
    }

    [System.Serializable]
    public class PowerSlot
    {
        [SerializeField] private bool isUnlocked;
        [SerializeField] private bool isOccupied;

        public bool IsUnlocked => isUnlocked;
        public bool IsOccupied => isOccupied;
        public bool IsAvailable => isUnlocked && !isOccupied;

        public void Unlock()
        {
            isUnlocked = true;
        }

        public void Lock()
        {
            isUnlocked = false;
            isOccupied = false;
        }

        public bool TryOccupy()
        {
            if (!IsAvailable)
            {
                return false;
            }

            isOccupied = true;
            return true;
        }

        public void Release()
        {
            isOccupied = false;
        }
    }

    [Header("Length Milestones")]
    [SerializeField]
    private List<LengthMilestone> lengthMilestones = new();

    [Header("Power Slots")]
    [SerializeField]
    private List<PowerSlot> powerSlots = new();

    private int currentLength;

    public int CurrentLength => currentLength;

    public int UnlockedSlotCount
    {
        get
        {
            int count = 0;

            foreach (PowerSlot slot in powerSlots)
            {
                if (slot.IsUnlocked)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public int OccupiedSlotCount
    {
        get
        {
            int count = 0;

            foreach (PowerSlot slot in powerSlots)
            {
                if (slot.IsOccupied)
                {
                    count++;
                }
            }

            return count;
        }
    }

    public int AvailableSlotCount =>
        UnlockedSlotCount - OccupiedSlotCount;

    private void Awake()
    {
        UpdateUnlockedSlots();
    }

    public void UpdateLength(int newLength)
    {
        currentLength = Mathf.Max(0, newLength);
        UpdateUnlockedSlots();
    }

    private void UpdateUnlockedSlots()
    {
        int targetUnlockedSlots = 0;

        foreach (LengthMilestone milestone in lengthMilestones)
        {
            if (currentLength >= milestone.requiredLength)
            {
                targetUnlockedSlots = Mathf.Max(
                    targetUnlockedSlots,
                    milestone.unlockedSlots
                );
            }
        }

        targetUnlockedSlots = Mathf.Clamp(
            targetUnlockedSlots,
            0,
            powerSlots.Count
        );

        for (int i = 0; i < powerSlots.Count; i++)
        {
            if (i < targetUnlockedSlots)
            {
                powerSlots[i].Unlock();
            }
            else
            {
                powerSlots[i].Lock();
            }
        }
    }

    public bool HasAvailableSlot()
    {
        return AvailableSlotCount > 0;
    }

    public int GetAvailableSlotIndex()
    {
        for (int i = 0; i < powerSlots.Count; i++)
        {
            if (powerSlots[i].IsAvailable)
            {
                return i;
            }
        }

        return -1;
    }

    public bool TryOccupySlot(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return false;
        }

        return powerSlots[slotIndex].TryOccupy();
    }

    public void ReleaseSlot(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return;
        }

        powerSlots[slotIndex].Release();
    }

    public bool IsSlotUnlocked(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return false;
        }

        return powerSlots[slotIndex].IsUnlocked;
    }

    public bool IsSlotOccupied(int slotIndex)
    {
        if (!IsValidSlotIndex(slotIndex))
        {
            return false;
        }

        return powerSlots[slotIndex].IsOccupied;
    }

    private bool IsValidSlotIndex(int slotIndex)
    {
        return slotIndex >= 0 && slotIndex < powerSlots.Count;
    }
}