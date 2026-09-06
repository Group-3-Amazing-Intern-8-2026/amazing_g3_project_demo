using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float boostSpeed = 10f;
    [SerializeField] private float rotationSpeed = 200f;

    [Header("Body & Tail Tracking")]
    [SerializeField] private float distanceBetweenSegments = 0.5f;

    [HideInInspector]
    public List<Vector2> positionHistory = new List<Vector2>();
    [HideInInspector]
    public bool isBoosting = false;

    private Camera mainCamera;
    private int bodySegmentCount = 0;

    public float DistanceBetweenSegments => distanceBetweenSegments;

    void Start()
    {
        mainCamera = Camera.main;
        positionHistory.Add(transform.position);
    }

    void Update()
    {
        HandleInput();
    }

    void FixedUpdate()
    {
        MoveHead();
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }

        float currentSpeed = isBoosting ? boostSpeed : moveSpeed;
        transform.Translate(Vector3.up * currentSpeed * Time.fixedDeltaTime);

        if (positionHistory.Count == 0 || Vector2.Distance(transform.position, positionHistory[0]) > 0.05f)
        {
            positionHistory.Insert(0, transform.position);

            int maxHistory = (bodySegmentCount + 1) * Mathf.CeilToInt(distanceBetweenSegments / 0.05f) + 50;
            if (positionHistory.Count > maxHistory)
            {
                positionHistory.RemoveAt(positionHistory.Count - 1);
            }
        }
    }

    public void UpdateSegmentCount(int count)
    {
        bodySegmentCount = count;
    }
}
