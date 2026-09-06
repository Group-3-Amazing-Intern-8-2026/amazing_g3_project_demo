using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float boostSpeed = 10f;
    public float rotationSpeed = 200f;

    [Header("Body & Tail Tracking")]
    public float distanceBetweenSegments = 0.5f;
    public int initialBodyCount = 5;
    public GameObject bodySegmentPrefab;

    [HideInInspector]
    public List<Vector2> positionHistory = new List<Vector2>();
    [HideInInspector]
    public bool isBoosting = false;

    private List<Transform> bodySegments = new List<Transform>();
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        positionHistory.Add(transform.position);

        // Instantiate initial body segments
        for (int i = 0; i < initialBodyCount; i++)
        {
            Grow();
        }
    }

    void Update()
    {
        HandleInput();
        MoveHead();
        UpdateBodySegments();
    }

    void HandleInput()
    {
        if (InputController.Instance != null)
        {
            isBoosting = InputController.Instance.IsBoosting;
        }
        else
        {
            isBoosting = Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space);
        }
    }

    void MoveHead()
    {
        // Rotate towards pointer position from InputController
        Vector2 screenPos = InputController.Instance != null ? InputController.Instance.PointerPosition : (Vector2)Input.mousePosition;
        Vector3 mousePos = mainCamera.ScreenToWorldPoint(screenPos);
        mousePos.z = 0f;
        Vector2 direction = (mousePos - transform.position).normalized;

        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Move forward
        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        // Record position history for body following
        if (positionHistory.Count == 0 || Vector2.Distance(transform.position, positionHistory[0]) > 0.1f)
        {
            positionHistory.Insert(0, transform.position);

            // Limit history size to prevent memory growth
            int maxHistory = (bodySegments.Count + 1) * Mathf.CeilToInt(distanceBetweenSegments / 0.1f) + 50;
            if (positionHistory.Count > maxHistory)
            {
                positionHistory.RemoveAt(positionHistory.Count - 1);
            }
        }
    }

    void UpdateBodySegments()
    {
        int pointIndex = 0;
        for (int i = 0; i < bodySegments.Count; i++)
        {
            float requiredDistance = distanceBetweenSegments * (i + 1);
            float accumulatedDistance = 0f;

            // Find the point in history that matches the required distance
            for (int j = 0; j < positionHistory.Count - 1; j++)
            {
                float dist = Vector2.Distance(positionHistory[j], positionHistory[j + 1]);
                if (accumulatedDistance + dist >= requiredDistance)
                {
                    float leftover = requiredDistance - accumulatedDistance;
                    Vector2 interpolatedPos = Vector2.Lerp(positionHistory[j], positionHistory[j + 1], leftover / dist);
                    bodySegments[i].position = interpolatedPos;
                    break;
                }
                accumulatedDistance += dist;
                pointIndex = j;
            }
        }
    }

    public void Grow()
    {
        Vector2 spawnPos = positionHistory.Count > 0 ? positionHistory[positionHistory.Count - 1] : (Vector2)transform.position;
        GameObject segment = null;

        if (bodySegmentPrefab != null)
        {
            segment = Instantiate(bodySegmentPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            // Create a simple default circle if prefab is not assigned
            segment = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            segment.transform.position = spawnPos;
            segment.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            Destroy(segment.GetComponent<Collider>());
        }

        bodySegments.Add(segment.transform);
    }
}
