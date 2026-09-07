using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boostSpeed = 10f;
    [SerializeField] private float rotationSpeed = 220f;

    [Header("Path Settings")]
    [SerializeField] private float pointSpacing = 0.05f;
    [SerializeField] private int maxPathPoints = 1500;
    
    [HideInInspector] public bool isBoosting = false;

    public float PointSpacing => pointSpacing;

    private Vector2[] pathBuffer;
    private int pathWriteIndex = 0;  
    private int pathLength = 0;    

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        pathBuffer = new Vector2[maxPathPoints];
        
        Vector2 startPos = transform.position;
        Vector2 startDir = -transform.up;
        for (int i = 0; i < maxPathPoints; i++)
        {
            pathBuffer[i] = startPos + startDir * (i * pointSpacing);
        }
        pathLength = maxPathPoints;
    }

    void Update()
    {
        HandleInput();
        MoveHead();
    }

    void HandleInput()
    {
        isBoosting = (InputController.Instance != null) 
            ? InputController.Instance.IsBoosting 
            : (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space));
    }
    void MoveHead()
    {
        RotateTowardsPointer();
        TranslateForward();
        RecordPathPoints();
    }
    private Vector2 GetPointerWorldPosition()
    {
        Vector2 screenPos = InputController.Instance != null 
            ? InputController.Instance.PointerPosition 
            : (Vector2)Input.mousePosition;

        return mainCamera.ScreenToWorldPoint(screenPos);
    }

    private void RotateTowardsPointer()
    {
        Vector2 targetPos = GetPointerWorldPosition();
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        if (direction == Vector2.zero) return;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);
        
        transform.rotation = Quaternion.RotateTowards(
            transform.rotation, 
            targetRotation, 
            rotationSpeed * Time.deltaTime
        );
    }

    private void TranslateForward()
    {
        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;
        transform.Translate(Vector3.up * (currentSpeed * Time.deltaTime));
    }

    private void RecordPathPoints()
    {
        float distFromLastPoint = Vector2.Distance(transform.position, pathBuffer[pathWriteIndex]);

        while (distFromLastPoint >= pointSpacing)
        {
            Vector2 dir = ((Vector2)transform.position - pathBuffer[pathWriteIndex]).normalized;
            Vector2 newPoint = pathBuffer[pathWriteIndex] + (dir * pointSpacing);

            pathWriteIndex = (pathWriteIndex + 1) % maxPathPoints;
            pathBuffer[pathWriteIndex] = newPoint;

            if (pathLength < maxPathPoints)
            {
                pathLength++;
            }

            distFromLastPoint = Vector2.Distance(transform.position, pathBuffer[pathWriteIndex]);
        }
    }

    public Vector2 GetPointAtDistance(float distanceBehindHead)
    {
        if (distanceBehindHead <= 0f || pathLength == 0) 
            return transform.position;

        Vector2 headPos = transform.position;
        Vector2 latestBufferPoint = pathBuffer[pathWriteIndex];
        float headToBufferDist = Vector2.Distance(headPos, latestBufferPoint);

        // Region 1: Target distance is between head and newest buffer point
        if (distanceBehindHead <= headToBufferDist)
        {
            float t = headToBufferDist > 0.0001f ? distanceBehindHead / headToBufferDist : 0f;
            return Vector2.Lerp(headPos, latestBufferPoint, t);
        }

        // Region 2: Target distance lies further back in circular buffer
        float remainingDist = distanceBehindHead - headToBufferDist;
        float steps = remainingDist / pointSpacing;

        int step0 = Mathf.FloorToInt(steps);
        float frac = steps - step0;

        if (step0 >= pathLength - 1)
        {
            int oldestIndex = (pathWriteIndex - (pathLength - 1) + maxPathPoints) % maxPathPoints;
            return pathBuffer[oldestIndex];
        }

        int index0 = (pathWriteIndex - step0 + maxPathPoints) % maxPathPoints;
        int index1 = (pathWriteIndex - (step0 + 1) + maxPathPoints) % maxPathPoints;

        return Vector2.Lerp(pathBuffer[index0], pathBuffer[index1], frac);
    }
}