using Unity.VisualScripting;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
    [SerializeField]
    private float PlayerCrouchHeight = 0.5f;
    [SerializeField]
    private float GlidingGravity = -0.3f;
    [SerializeField]
    private float fallingThreshold = 2.5f;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float playerAnimSpeed = .25f;

    [Space]

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private InputManager inputManager;
    private Transform camera;
    
    private int MoveXAnimatorFloat;
    private int MoveYAnimatorFloat;
    private int JumpAnim;

    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private float PlayerBaseHeight;
    private float BaseGravity;
    private float fallStartLevel;
    private bool isFalling = false;

    private void Start()
    {
        JumpAnim = Animator.StringToHash("Jump");
        MoveXAnimatorFloat = Animator.StringToHash("MaxX");
        MoveYAnimatorFloat = Animator.StringToHash("MaxY");
        camera = Camera.main.transform;
        controller = GetComponent<CharacterController>();
        inputManager = InputManager.Instance;
        playerSpeed = playerBaseSpeed;
        PlayerBaseHeight = gameObject.transform.localScale.y;
        BaseGravity = gravityValue;
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

        //Animação
        animator.SetFloat(MoveXAnimatorFloat, currentInputVector.x);
        animator.SetFloat(MoveYAnimatorFloat, currentInputVector.y);

        // Changes the height position of the player..
        if (inputManager.PlayerJumped() && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(JumpAnim, playerAnimSpeed);
        }

        
        //Corrida
        if (inputManager.isSprinting == true && inputManager.isCrouching == false && groundedPlayer)
        {
            playerSpeed = playerBaseSpeed * playerSprintMultiplier;
        }
        else
        {
            playerSpeed = playerBaseSpeed;
        }


        //Agachar
        if (inputManager.isCrouching == true && inputManager.isSprinting == false)
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, PlayerCrouchHeight, gameObject.transform.localScale.z);
        }
        else
        {
            gameObject.transform.localScale = new Vector3(gameObject.transform.localScale.x, PlayerBaseHeight, gameObject.transform.localScale.z);
        }


        //Mecanica de planar

        //Detectar se o jogadar está caindo
        if (!isFalling)
        {
            if(playerVelocity.y < 0)
            {
                fallStartLevel = transform.position.y;
                isFalling = true;
            }
        }
        else
        {
            if (transform.position.y < fallStartLevel - fallingThreshold)
            {
                Debug.Log("Está Caindo");
                isFalling = false;
            }
        }
        if(groundedPlayer) isFalling = false;

        //Planar
        if (inputManager.isGliding == true && !groundedPlayer && isFalling)
        {
            playerVelocity.y = -1;
        }
        
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);


    }
}
