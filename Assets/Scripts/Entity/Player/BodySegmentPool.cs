using UnityEngine;

public class BodySegmentPool : ObjectPoolBase<BodySegment>
{
    [SerializeField] private Transform bodySegmentParent;

    protected override BodySegment CreateItem()
    {
        Transform targetParent = bodySegmentParent != null ? bodySegmentParent : transform;
        BodySegment segmentComponent;
        if (prefab != null)
        {
            segmentComponent = Instantiate(prefab, targetParent);
        }
        else
        {
            GameObject segment = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            segment.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            segment.transform.SetParent(targetParent);
            Collider col = segment.GetComponent<Collider>();
            if (col != null) Destroy(col);
            segmentComponent = segment.AddComponent<BodySegment>();
        }
        return segmentComponent;
    }
}
