// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// public class PlayerMovement : MonoBehaviour
// {
//     public float moveSpeed = 6f;
//     public float jumpForce = 7f;
//     public Transform groundCheck;
//     public float groundDistance = 0.4f;
//     public LayerMask groundMask;

//     private Rigidbody rb;
//     private Vector3 moveInput;
//     private bool isGrounded;

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }

//     void Update()
//     {
//         isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

//         float moveX = Input.GetAxis("Horizontal");
//         float moveZ = Input.GetAxis("Vertical");
//         moveInput = transform.right * moveX + transform.forward * moveZ;

//         if (Input.GetButtonDown("Jump") && isGrounded)
//         {
//             rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
//             rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
//         }
//     }

//     void FixedUpdate()
//     {
//         Vector3 velocity = moveInput.normalized * moveSpeed;
//         Vector3 currentVelocity = rb.linearVelocity;
//         rb.linearVelocity = new Vector3(velocity.x, currentVelocity.y, velocity.z);
//     }
// }


using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    //public Camera playerCamera;

    //New stuff for testing - Michael

    public GameObject cameraRigPrefab;
    private GameObject camInstance;
    public Transform cameraFollowTarget;

    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float wallrunSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;   
    public NetworkVariable<bool> isAlive = new NetworkVariable<bool>(true, default, NetworkVariableWritePermission.Server);
    Vector3 moveDirection;
    Rigidbody rb;

    public MovementState state;


    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        air
    }

    public bool wallrunning;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startYScale = transform.localScale.y;
        ResetJump();
    }

    // For Networking

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
            
        grounded = Physics.Raycast(transform.position, Vector3.down, 0.3f, whatIsGround);

        //New stuff for cam testing
        camInstance = transform.Find("CameraRig").gameObject;

        ThirdPersonCam camScript = camInstance.GetComponent<ThirdPersonCam>();
        camScript.target = cameraFollowTarget;
        camScript.orientation = orientation;

        Camera cam = camInstance.GetComponentInChildren<Camera>();
        if (cam != null)
        {
            cam.enabled = true;
            cam.GetComponent<AudioListener>().enabled = true;
        }

        ResetJump();
        startYScale = transform.localScale.y;

        /* Old Is owner script, uncomment if this doesn't work
         if (IsOwner)
        {
            playerCamera.enabled = true;
            playerCamera.GetComponent<AudioListener>().enabled = true;
            ResetJump();
        }
        else
        {
            playerCamera.enabled = false;
            playerCamera.GetComponent<AudioListener>().enabled = false;
        }
        startYScale = transform.localScale.y;
        */
    }

    private void Update()
    {

        if (!IsOwner) return;

        // Ground check
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        // Debug.Log($"{grounded} Ready to jump {readyToJump}");

        if (isAlive.Value)
        {
            MyInput();     // only allow player movement input when still alive
            SpeedControl();
            StateHandler();

            // handle drag
            if (grounded)
                rb.linearDamping = groundDrag;
            else
                rb.linearDamping = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
    }

    private void MyInput()
    {
        grounded = Physics.CheckSphere(groundCheck.position, groundDistance, whatIsGround);
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // Start crouching
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // Stop crouching
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Wallrunning
        if (wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallrunSpeed;
        }

        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        if (grounded && Input.GetKey(sprintKey) && state != MovementState.crouching)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        // Mode - Air
        else 
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculte movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on ground
        if (grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
