using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float playerBaseSpeed = 2.0f;
    private float playerSpeed;
    [SerializeField]
    private float playerSprintMultiplier = 4.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float smoothInputSpeed = .2f;
    [Space]

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManager inputManager;
    private Transform camera;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    
    private void Start()
    {
        camera = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        playerSpeed = playerBaseSpeed;
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 movement = inputManager.GetPlayerMovement();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, movement, ref smoothInputVelocity, smoothInputSpeed);
        Vector3 move = new Vector3(currentInputVector.x, 0f, currentInputVector.y);
        move = camera.forward * move.z + camera.right * move.x;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (InputManager.isSprinting == true)
        {
            playerSpeed = playerBaseSpeed * playerSprintMultiplier;
        }
        else
        {
            playerSpeed = playerBaseSpeed;
        }
        // Changes the height position of the player..
        if (inputManager.PlayerJumped() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
