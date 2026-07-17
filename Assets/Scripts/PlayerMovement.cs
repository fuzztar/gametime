using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float walkSpeed = 6f;
    public float runSpeed = 9f;
    public float crouchSpeed = 3f;
    private float currentSpeed;

    [SerializeField] private Transform mainCamera;
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private bool enableCameraBob = true;
    private Vector3 cameraStartPos;
    private Vector3 move;
    [SerializeField, Range (0,30)] private float cameraBobSpeed;
    [SerializeField, Range(0,0.1f)] private float cameraBobAmount = 0.005f;
    [SerializeField] private float cameraSmooth = 10f;

    // Start is called before the first frame update
    void Awake()
    {
        cameraStartPos = mainCamera.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        // Get keyboard input from WASD or Arrow Keys
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the movement direction
        move = transform.right * horizontalInput + transform.forward * verticalInput;

        // Change the speed & camera speed to match whether the player is sprinting, crouching, or neither, and bob the camera based on that
        SetSpeed();
        cameraBobSpeed = currentSpeed * 2;

        // Move the character
        controller.Move(currentSpeed * Time.deltaTime * move);
    }

    void LateUpdate()
    {
        if (mainCamera == null) return;

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
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            currentSpeed = runSpeed;
        } else if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = crouchSpeed;
        } else
        {
            currentSpeed = walkSpeed;
        }
    }

    private Vector3 CameraBob()
    {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Lerp(pos.y, Mathf.Sin(Time.time * cameraBobSpeed) * cameraBobAmount, cameraSmooth * Time.deltaTime);
        pos.x += Mathf.Lerp(pos.x, Mathf.Cos(Time.time * cameraBobSpeed / 2) * cameraBobAmount * 2, cameraSmooth * Time.deltaTime);
        mainCamera.localPosition += pos;

        return pos;
    }

    private void ResetPosition()
    {
        if (mainCamera.localPosition == cameraStartPos) return;
        mainCamera.localPosition = Vector3.Lerp(mainCamera.localPosition, cameraStartPos, 1 * Time.deltaTime);
    }
}
