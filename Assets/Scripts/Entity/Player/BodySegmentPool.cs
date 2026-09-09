using UnityEngine;

public class BodySegmentPool : ObjectPoolBase<BodySegment>
{
    [SerializeField] private Transform bodySegmentParent;

    protected override BodySegment CreateItem()
    {
        return Instantiate(prefab, bodySegmentParent != null ? bodySegmentParent : transform);
    }
}