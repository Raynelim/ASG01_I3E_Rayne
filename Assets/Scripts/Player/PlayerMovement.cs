using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Mouse Settings")]
    public float mouseSensitivity = 100f;
    public Transform cameraHolder;

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float sprintMultiplier = 1.5f; // NEW: Sprint multiplier
    public float jumpForce = 5f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    private Rigidbody rb;
    private float xRot = 0f;
    private bool isGrounded;

    private bool jumpRequested = false;
    private bool isWalking = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpRequested = true;
            AudioManager.Instance.PlaySound("Jump");
        }
    }

    void FixedUpdate()
    {
        HandleMouseLook();
        HandleMovement();

        rb.angularVelocity = Vector3.zero;
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotate player (yaw)
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, mouseX, 0f));

        // Rotate cameraHolder (pitch)
        xRot -= mouseY;
        xRot = Mathf.Clamp(xRot, -90f, 90f);
        cameraHolder.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = transform.right * x + transform.forward * z;

        // Check if sprinting
        float currentSpeed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift) && z > 0)
        {
            currentSpeed *= sprintMultiplier;
        }

        Vector3 targetVelocity = moveDir.normalized * currentSpeed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;

        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Jumping (handle cached input)
        if (isGrounded && jumpRequested)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpRequested = false;
        }

        // Reset jump request if player is not grounded
        if (!isGrounded)
        {
            jumpRequested = false;
        }

        bool isMoving = (x != 0 || z != 0);

        if (isGrounded && isMoving)
        {
            if (!isWalking)
            {
                isWalking = true;
                AudioManager.Instance.PlaySound("Step");
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                AudioManager.Instance.StopSound("Step");
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}
