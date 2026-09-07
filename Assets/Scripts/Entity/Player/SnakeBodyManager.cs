using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyManager : MonoBehaviour
{
    [Header("Hierarchy Setup")]
    [SerializeField] private Transform bodyContainer;
    [SerializeField] private SnakeMovement snakeMovement;

    [Header("Body Settings")]
    [SerializeField] private float spacingBetweenSegments = 0.3f;
    [SerializeField] private int initialBodyCount = 5;
    [SerializeField] private BodySegmentPool bodySegmentPool;

    private List<Transform> bodySegments = new List<Transform>();

    public int SegmentCount => bodySegments.Count;

    void Start()
    {

        for (int i = 0; i < initialBodyCount; i++)
        {
            Grow();
        }
    }
    void LateUpdate()
    {
        UpdateBodyPositionsAndRotations();
    }

    void UpdateBodyPositionsAndRotations()
    {
        for (int i = 0; i < bodySegments.Count; i++)
        {
            float lagDistance = (i + 1) * spacingBetweenSegments;
            Vector2 segmentPos = snakeMovement.GetPointAtDistance(lagDistance);
            bodySegments[i].position = new Vector3(segmentPos.x, segmentPos.y, 0f);

            Vector2 targetLookPos = (i == 0) ? (Vector2)snakeMovement.transform.position : (Vector2)bodySegments[i - 1].position;
            Vector2 lookDir = targetLookPos - segmentPos;

            if (lookDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                bodySegments[i].rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
    }

    public void Grow()
    {
        BodySegment segment = bodySegmentPool.Get();
        segment.transform.SetParent(bodyContainer ?? transform);

        float lagDistance = (bodySegments.Count + 1) * spacingBetweenSegments;
        Vector2 spawnPos = snakeMovement.GetPointAtDistance(lagDistance);
        segment.transform.position = new Vector3(spawnPos.x, spawnPos.y, 0f);
        bodySegments.Add(segment.transform);
    }

    public void Shrink()
    {
        if (bodySegments.Count == 0) return;

        int lastIndex = bodySegments.Count - 1;
        BodySegment segment = bodySegments[lastIndex].GetComponent<BodySegment>();
        bodySegments.RemoveAt(lastIndex);
        if (segment != null)
        {
            bodySegmentPool.Release(segment);
        }
    }
}