using System.Collections.Generic;
using UnityEngine;

public class SnakeBodyManager : MonoBehaviour
{
    [Header("Body Settings")]
    [SerializeField] private int initialBodyCount = 5;
    [SerializeField] private BodySegmentPool bodySegmentPool;
    [SerializeField] private float followSmoothing = 25f;

    private SnakeMovement snakeMovement;
    private List<Transform> bodySegments = new List<Transform>();

    public int SegmentCount => bodySegments.Count;

    void Start()
    {
        snakeMovement = GetComponentInParent<SnakeMovement>();

        if (bodySegmentPool == null)
        {
            bodySegmentPool = GetComponent<BodySegmentPool>();
            if (bodySegmentPool == null)
            {
                bodySegmentPool = FindFirstObjectByType<BodySegmentPool>();
            }
        }

        for (int i = 0; i < initialBodyCount; i++)
        {
            Grow();
        }
    }

    void FixedUpdate()
    {
        UpdateBodySegments();
    }

    void UpdateBodySegments()
    {
        if (snakeMovement == null || snakeMovement.positionHistory.Count == 0) return;

        snakeMovement.UpdateSegmentCount(bodySegments.Count);

        int historyIndex = 0;
        float accumulatedDistance = 0f;

        for (int i = 0; i < bodySegments.Count; i++)
        {
            float requiredDistance = snakeMovement.DistanceBetweenSegments * (i + 1);

            while (historyIndex < snakeMovement.positionHistory.Count - 1)
            {
                float dist = Vector2.Distance(snakeMovement.positionHistory[historyIndex], snakeMovement.positionHistory[historyIndex + 1]);
                if (accumulatedDistance + dist >= requiredDistance)
                {
                    break;
                }
                accumulatedDistance += dist;
                historyIndex++;
            }

            Vector2 targetPos = bodySegments[i].position;
            if (historyIndex < snakeMovement.positionHistory.Count - 1)
            {
                float dist = Vector2.Distance(snakeMovement.positionHistory[historyIndex], snakeMovement.positionHistory[historyIndex + 1]);
                float leftover = requiredDistance - accumulatedDistance;
                targetPos = Vector2.Lerp(snakeMovement.positionHistory[historyIndex], snakeMovement.positionHistory[historyIndex + 1], dist > 0f ? leftover / dist : 0f);
            }
            else if (snakeMovement.positionHistory.Count > 0)
            {
                targetPos = snakeMovement.positionHistory[snakeMovement.positionHistory.Count - 1];
            }

            bodySegments[i].position = Vector2.Lerp(bodySegments[i].position, targetPos, followSmoothing * Time.fixedDeltaTime);
        }
    }

    public void Grow()
    {
        if (bodySegmentPool == null) return;

        List<Vector2> history = snakeMovement != null ? snakeMovement.positionHistory : null;
        Vector2 spawnPos = (history != null && history.Count > 0) ? history[history.Count - 1] : (Vector2)transform.position;
        BodySegment segment = bodySegmentPool.Get();
        segment.transform.position = spawnPos;
        bodySegments.Add(segment.transform);
    }

    public void Shrink()
    {
        if (bodySegmentPool == null) return;

        if (bodySegments.Count > 0)
        {
            Transform segmentTransform = bodySegments[bodySegments.Count - 1];
            bodySegments.RemoveAt(bodySegments.Count - 1);
            BodySegment segment = segmentTransform.GetComponent<BodySegment>();
            if (segment != null)
            {
                bodySegmentPool.Release(segment);
            }
        }
    }
}
