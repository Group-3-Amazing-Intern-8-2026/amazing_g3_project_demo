using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boostSpeed = 10f;
    [SerializeField] private float rotationSpeed = 200f;

    [Header("Eating Settings")]
    [SerializeField] private float eatRadius = 0.6f;
    [SerializeField] private LayerMask foodLayer;

    [Header("Body & Tail Tracking")]
    [SerializeField] private float distanceBetweenSegments = 0.5f;
    [SerializeField] private int initialBodyCount = 5;
    [SerializeField] private GameObject bodySegmentPrefab;
    [SerializeField] private Transform bodySegmentParent;

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
        CheckFoodOverlap();
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

        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;
        transform.Translate(Vector3.up * currentSpeed * Time.deltaTime);

        if (positionHistory.Count == 0 || Vector2.Distance(transform.position, positionHistory[0]) > 0.1f)
        {
            positionHistory.Insert(0, transform.position);

            int maxHistory = (bodySegments.Count + 1) * Mathf.CeilToInt(distanceBetweenSegments / 0.1f) + 50;
            if (positionHistory.Count > maxHistory)
            {
                positionHistory.RemoveAt(positionHistory.Count - 1);
            }
        }
    }

    void CheckFoodOverlap()
    {
        Collider2D[] hitFoods = Physics2D.OverlapCircleAll(transform.position, eatRadius, foodLayer);

        for (int i = 0; i < hitFoods.Length; i++)
        {
            Food food = hitFoods[i].GetComponent<Food>();
            if (food != null)
            {
                Grow();
                food.OnEaten();
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

        Transform targetParent = bodySegmentParent != null ? bodySegmentParent : transform;

        if (bodySegmentPrefab != null)
        {
            segment = Instantiate(bodySegmentPrefab, spawnPos, Quaternion.identity, targetParent);
        }
        else
        {
            segment = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            segment.transform.position = spawnPos;
            segment.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
            segment.transform.SetParent(targetParent);
            Destroy(segment.GetComponent<Collider>());
        }

        bodySegments.Add(segment.transform);
    }

    // Visual helper: Draws the eat radius as a green circle in the Unity Scene View
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, eatRadius);
    }
}