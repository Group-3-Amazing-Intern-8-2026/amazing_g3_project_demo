using System;
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

    [Header("Growth Progress & Scaling")]
    [SerializeField] private float segmentProgress = 0f;
    [SerializeField] private float minTailScale = 0.2f;
    [SerializeField] private float maxTailScale = 1.0f;

    public float SegmentProgress => segmentProgress;
    public int SegmentCount => bodySegments.Count;

    public event Action<float> OnSegmentProgressUpdated;

    private readonly List<Transform> bodySegments = new List<Transform>();

    private void Start()
    {
        for (int i = 0; i < initialBodyCount; i++)
        {
            Grow();
        }

        UpdateTailScale();
    }

    private void LateUpdate()
    {
        UpdateBodyPositionsAndRotations();
    }

    private void UpdateBodyPositionsAndRotations()
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

    public void AddGrowthProgress(float progressPercent)
    {
        segmentProgress += progressPercent;

        while (segmentProgress >= 100f)
        {
            segmentProgress -= 100f;
            Grow();
        }

        UpdateTailScale();
        OnSegmentProgressUpdated?.Invoke(segmentProgress);
    }

    private void UpdateTailScale()
    {
        if (bodySegments.Count == 0) return;
        for (int i = 0; i < bodySegments.Count - 1; i++)
        {
            bodySegments[i].localScale = Vector3.one * maxTailScale;
        }

        float t = Mathf.Clamp01(segmentProgress / 100f);
        float currentScale = Mathf.Lerp(minTailScale, maxTailScale, t);
        bodySegments[bodySegments.Count - 1].localScale = Vector3.one * currentScale;
    }

    public void Grow()
    {
        BodySegment segment = bodySegmentPool.Get();
        segment.transform.SetParent(bodyContainer != null ? bodyContainer : transform);

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

        UpdateTailScale();
    }
}