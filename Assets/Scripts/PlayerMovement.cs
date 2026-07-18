using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    [Header("Movement Speeds")]
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float crouchSpeed = 3f;
    private float currentSpeed;


    [Header("Gravity")]
    [SerializeField] private float gravity = -20f;
    private float verticalVelocity;



    [Header("Camera")]
    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform cameraHolder;

    [SerializeField] private bool enableCameraBob = true;
    private Vector3 cameraStartPos;

    private Vector3 move;

    [SerializeField, Range(0, 30)]
    private float cameraBobSpeed;

    [SerializeField, Range(0, 0.1f)]
    private float cameraBobAmount = 0.005f;

    [SerializeField]
    private float cameraSmooth = 10f;



    [Header("Footsteps")]
    [SerializeField] private AudioSource footstepAudioSource;

    [SerializeField] private List<AudioClip> footstepClips = new();

    [SerializeField] private float walkStepInterval = 0.5f;
    [SerializeField] private float runStepInterval = 0.35f;
    [SerializeField] private float crouchStepInterval = 0.7f;

    [SerializeField] private float footstepVolume = 1f;

    private float footstepTimer;



    void Awake()
    {
        cameraStartPos = mainCamera.localPosition;
    }



    void Update()
    {
        // Get keyboard input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");


        // Calculate movement direction
        move = transform.right * horizontalInput +
               transform.forward * verticalInput;


        SetSpeed();

        cameraBobSpeed = currentSpeed * 2;



        // Gravity
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;



        // Combine movement + gravity
        Vector3 finalMove = move * currentSpeed;

        finalMove.y = verticalVelocity;


        controller.Move(
            finalMove * Time.deltaTime
        );



        HandleFootsteps(
            horizontalInput,
            verticalInput
        );
    }



    void LateUpdate()
    {
        if (mainCamera == null)
            return;


        if (enableCameraBob)
        {
            if (move != Vector3.zero)
            {
                CameraBob();
            }
            else
            {
                ResetPosition();
            }
        }
    }



    void SetSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift) &&
            !Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = runSpeed;
        }
        else if (Input.GetKey(KeyCode.LeftControl) &&
                 !Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = crouchSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
        }
    }



    void HandleFootsteps(float horizontal, float vertical)
    {
        // Not moving
        if (Mathf.Abs(horizontal) < 0.1f &&
            Mathf.Abs(vertical) < 0.1f)
        {
            footstepTimer = 0;
            return;
        }


        // Not grounded
        if (!controller.isGrounded)
            return;



        footstepTimer -= Time.deltaTime;


        if (footstepTimer <= 0)
        {
            PlayFootstep();

            footstepTimer = GetStepInterval();
        }
    }



    void PlayFootstep()
    {
        if (footstepAudioSource == null)
            return;


        if (footstepClips.Count == 0)
            return;


        AudioClip clip =
            footstepClips[
                Random.Range(
                    0,
                    footstepClips.Count
                )
            ];


        footstepAudioSource.PlayOneShot(
            clip,
            footstepVolume
        );
    }



    float GetStepInterval()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            return runStepInterval;
        }


        if (Input.GetKey(KeyCode.LeftControl))
        {
            return crouchStepInterval;
        }


        return walkStepInterval;
    }



    private Vector3 CameraBob()
    {
        Vector3 pos = Vector3.zero;


        pos.y += Mathf.Lerp(
            pos.y,
            Mathf.Sin(Time.time * cameraBobSpeed)
            * cameraBobAmount,
            cameraSmooth * Time.deltaTime
        );


        pos.x += Mathf.Lerp(
            pos.x,
            Mathf.Cos(Time.time * cameraBobSpeed / 2)
            * cameraBobAmount * 2,
            cameraSmooth * Time.deltaTime
        );


        mainCamera.localPosition += pos;


        return pos;
    }

    public void ResetCamera()
    {
        transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler( 0, 0, 0 );
    }

    private void ResetPosition()
    {
        if (mainCamera.localPosition == cameraStartPos)
            return;


        mainCamera.localPosition =
            Vector3.Lerp(
                mainCamera.localPosition,
                cameraStartPos,
                1 * Time.deltaTime
            );
    }
}