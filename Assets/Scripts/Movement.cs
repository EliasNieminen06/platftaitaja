using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public static Player instance;
    [Header("Player Camera")]
    public Camera playerCamera;
    [Header("Movement Variables")]
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 5f;
    public float gravity = 10f;
    Vector3 moveDirection = Vector3.zero;
    [Header("Movement Toggles")]
    public bool canMove = true;
    public bool canRun = true;
    public bool canJump = true;
    [Header("Animator")]
    public Animator anim;
    [Header("Character Controller")]
    CharacterController characterController;
    [Header("Fall Respawn")]
    public bool fallRespawn = false;
    public float fallDistance = -15f;
    public Transform checkpoint;
    [Header("Walk Sound")]
    public AudioClip walkSound;
    public AudioClip runSound;
    AudioSource walkSoundSource;
    [Range(0f, 1f)] public float walkVolume = 1f;
    [Range(0f, 2f)] public float walkPitch = 1f;
    public bool walkLoop = true;
    [Header("Jump Sound")]
    public AudioClip jumpSound;
    AudioSource jumpSoundSource;
    [Range(0f, 1f)] public float jumpVolume = 1f;
    [Range(0f, 2f)] public float jumpPitch = 1f;
    public bool jumpLoop = false;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if (jumpSound != null )
        {
            jumpSoundSource = gameObject.AddComponent<AudioSource>();
            jumpSoundSource.clip = jumpSound;
            jumpSoundSource.volume = jumpVolume;
            jumpSoundSource.pitch = jumpPitch;
            jumpSoundSource.loop = jumpLoop;
            jumpSoundSource.playOnAwake = false;
        }
        if (walkSound != null )
        {
            walkSoundSource = gameObject.AddComponent<AudioSource>();
            walkSoundSource.clip = walkSound;
            walkSoundSource.volume = walkVolume;
            walkSoundSource.pitch = walkPitch;
            walkSoundSource.loop = walkLoop;
            walkSoundSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (fallRespawn)
        {
            if (transform.position.y < fallDistance)
            {
                characterController.enabled = false;
                transform.position = checkpoint.position;
                moveDirection.y = 0;
                characterController.enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        TPPlayerMovement();
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
            anim.SetTrigger("jump");
            jumpSoundSource.Play();
        }
        else
        {
            moveDirection.y = movementDirectionY;
            anim.speed = 1;
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

        if (new Vector3(moveDirection.x, 0, moveDirection.z).magnitude > 0.1f && characterController.isGrounded)
        {
            anim.SetBool("run", true);
            anim.speed = characterController.velocity.magnitude / walkSpeed;
            if (isRunning)
            {
                if (!walkSoundSource.isPlaying || walkSoundSource.clip != runSound)
                {
                    walkSoundSource.clip = runSound;
                    walkSoundSource.Play();
                }
            }
            else
            {
                if (!walkSoundSource.isPlaying || walkSoundSource.clip != walkSound)
                {
                    walkSoundSource.clip = walkSound;
                    walkSoundSource.Play();
                }
            }
        }
        else
        {
            anim.SetBool("run", false);
            anim.speed = 1;
            walkSoundSource.Stop();
        }

    }
}