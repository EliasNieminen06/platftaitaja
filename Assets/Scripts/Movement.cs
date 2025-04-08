using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player instance;
    public Camera playerCamera;
    public Transform camPivot;

    public Vector3 tPOffset;

    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 5f;
    public float gravity = 10f;


    public float lookSpeed = 2f;
    public float lookXLimit = 90f;


    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;
    float rotationY = 0;

    public bool canMove = true;
    public bool canRun = true;
    public bool canJump = true;

    public bool isFirstPerson = true;


    CharacterController characterController;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        if (isFirstPerson)
        {
            playerCamera.transform.position = camPivot.position;
        }
        else
        {
            playerCamera.transform.position = camPivot.position+tPOffset;
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (canMove)
        {
            if (isFirstPerson)
            {
                FirstPersonCam();
            }
            else
            {
                ThirdPersonCam();
            }
        }
    }

    void FixedUpdate()
    {
        if (isFirstPerson)
        {
            FPPlayerMovement();
        }
        else
        {
            TPPlayerMovement();
        }
    }

    void FPPlayerMovement()
    {
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning && canRun ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning && canRun ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && canJump && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);
    }

    void FirstPersonCam()
    {
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    void TPPlayerMovement()
    {
        // Get the camera's forward and right directions
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        // Flatten the forward and right vectors so the player only moves on the X-Z plane
        forward.y = 0;
        right.y = 0;

        // Normalize the vectors (optional, but good practice)
        forward.Normalize();
        right.Normalize();

        // Get movement input
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning && canRun ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning && canRun ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;

        // Calculate the movement direction based on input
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (Input.GetButton("Jump") && canMove && canJump && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character
        characterController.Move(moveDirection * Time.deltaTime);

        // Rotate the player to face the direction of movement
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            // Calculate the target rotation based on movement direction
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0, moveDirection.z));

            // Smoothly rotate the player toward the movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10 * Time.deltaTime);
        }
    }

    void ThirdPersonCam()
    {
        
    }
}