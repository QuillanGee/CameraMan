using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FirstPersonCharacterMovement : MonoBehaviour
{
    // Player Control Settings
    public float walkSpeed = 3.0f;            // Movement speed
    private float gravity = -28.0f;           // Custom gravity force
    private float mouseSensitivity = 200.0f;  // Mouse sensitivity for look around
    private float groundDrag = 11.0f;

    [SerializeField] private Transform orientation;
    private float rotationX = 0.0f;           // Pitch rotation (up-down)
    private float rotationY = 0.0f;
    private float moveX = 0.0f;
    private float moveY = 0.0f;
    private Vector3 moveDirection = Vector3.zero; // Direction of player movement

    // Ground Check
    public Transform groundCheck;
    private float groundDistance = 0.3f;
    public LayerMask groundLayer;
    public LayerMask blockLayer;
    private bool isGrounded = true;

    // Jump Parameters
    private float jumpForce = 13.0f;          // Adjusted for balance
    private float jumpCooldown = 0.25f;
    private float airMultiplier = 0.3f;
    private bool readyToJump = true;

    //BouncePad
    private float bouncePadForce = 30.0f;
    
    private CinemachineVirtualCamera fpc;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fpc = GetComponentInChildren<CinemachineVirtualCamera>();

        // Lock the cursor to the center of the screen and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Disable Rigidbody's built-in gravity
        rb.useGravity = false;
        rotationY = transform.eulerAngles.y; // Match the initial rotation of the character
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);

        EventManager.instance.OnPauseGamePlay += HandlePause;
    }

    private void FixedUpdate()
    {
        ApplyGravity(); // Custom gravity
    }

    void Update()
    {
        // Check if grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer) 
                     || Physics.CheckSphere(groundCheck.position, groundDistance, blockLayer);

        rb.drag = isGrounded ? groundDrag : 0;

        MyInput();
        SpeedControl();
        MoveCharacter();

        // Mouse look
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);
        fpc.transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }

    private void MyInput()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && isGrounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void MoveCharacter()
    {
        moveDirection = orientation.right * moveX + orientation.forward * moveY;

        if (isGrounded)
        {
            rb.AddForce(moveDirection.normalized * walkSpeed * 10f, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * walkSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0.0f, rb.velocity.z);

        if (flatVel.magnitude > walkSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * walkSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void ApplyGravity()
    {
        // Apply a constant downward force
        rb.AddForce(Vector3.up * gravity, ForceMode.Acceleration);
    }

    private void Jump()
    {
        // Reset Y velocity to ensure consistent jump height
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("BouncePad"))
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * bouncePadForce, ForceMode.Impulse);
        }
        
        if (other.gameObject.CompareTag("DoorToUnlock"))
        {
            EventManager.instance.UnlockDoor();
        }
    }

    private void HandlePause(object sender, bool isPaused)
    {
        if (isPaused)
        {
            rb.useGravity = false;  // Disable gravity
            rb.constraints = RigidbodyConstraints.FreezePosition;  // Freeze all position axes
        }
        else
        {
            rb.useGravity = true;  // Re-enable gravity
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }
}
