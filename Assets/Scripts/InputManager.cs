using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    private static InputManager _instance;
    public bool isSprinting = false;
    public bool isCrouching = false;
    public bool isGliding = false;

    public static InputManager Instance
    {
        get { return _instance; }
    }

    private PlayerControls playerControls;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        playerControls = new PlayerControls();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerControls.Ground.Sprint.performed += ctx => PlayerSprintOn();
        playerControls.Ground.Sprint.canceled += ctx => PlayerSprintOff();
        playerControls.Ground.Crouch.performed += ctx => PlayerCrouchOn();
        playerControls.Ground.Crouch.canceled += ctx => PlayerCrouchOff();
        playerControls.Ground.Jump.performed += ctx => PlayerGlidingOn();
        playerControls.Ground.Jump.canceled += ctx => PlayerGlidingOff();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    public Vector2 GetPlayerMovement()
    {
        return playerControls.Ground.Movement.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return playerControls.Ground.Mouse.ReadValue<Vector2>();
    }

    public bool PlayerJumped()
    {
        return playerControls.Ground.Jump.triggered;
    }
    private void PlayerSprintOn()
    {
        isSprinting = true;
    }
    public void PlayerSprintOff()
    {
        isSprinting = false;
    }
    public void PlayerCrouchOn()
    {
        isCrouching = true;
    }
    public void PlayerCrouchOff()
    {
        isCrouching = false;
    }

    public void PlayerGlidingOn()
    {
        isGliding = true;
    }
    public void PlayerGlidingOff()
    {
        isGliding = false;
    }
}
