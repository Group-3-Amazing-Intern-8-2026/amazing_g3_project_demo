using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour, PlayerInput.IGameplayActions
{
    public static InputController Instance { get; private set; }

    private PlayerInput playerInput;

    public Vector2 PointerPosition { get; private set; }
    public bool IsBoosting { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerInput = new PlayerInput();
        playerInput.Gameplay.SetCallbacks(this);
    }

    private void OnEnable()
    {
        playerInput?.Gameplay.Enable();
    }

    private void OnDisable()
    {
        playerInput?.Gameplay.Disable();
    }

    private void OnDestroy()
    {
        playerInput?.Dispose();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        PointerPosition = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsBoosting = true;
        }
        else if (context.canceled)
        {
            IsBoosting = false;
        }
    }
}
