using System;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // No longer needed: private Rigidbody2D rb2D;
    public InputActionAsset inputActions;
    public float moveSpeed = 5f; // Adjusted for Transform.Translate (meters per second)
    public float jumpForce = 8f; // Adjusted for Transform.Translate (initial jump velocity)
    public float Strength = 5f;
    private InputAction moveAction;
    private InputAction HHA;
    private InputAction HLA;
    private InputAction LHA;
    private InputAction LLA;
    private Vector2 moveInput;

    // Manual physics variables
    public float gravity = -20f; // Gravity strength
    private Vector2 currentVelocity; // To store our custom velocity
    public LayerMask groundLayer; // Assign your ground layer in the Inspector

    // Check Transforms and Radius
    public Transform groundCheck; // An empty GameObject child at the player's feet
    public Transform leftWallCheck; // An empty GameObject child on the player's left side
    public Transform rightWallCheck; // An empty GameObject child on the player's right side
    public float groundCheckRadius = 0.2f; // Radius for ground detection (also used for wall checks for simplicity)

    // Reference to the player's main collider for BoxCast/CapsuleCast
    private Collider2D playerCollider;

    [SerializeField] private bool Grounded;
    [SerializeField] private bool againstLeftWall;
    [SerializeField] private bool againstRightWall;

    public DetectTriggerOverlay HHB;
    public DetectTriggerOverlay LHB;
    private float cd;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public float selectionTimer;

    private void OnEnable()
    {
        // Find the action map and move action
        var actionMap = inputActions.FindActionMap("BasicControls");
        moveAction = actionMap.FindAction("Movement");
        HHA = actionMap.FindAction("HHA");
        HLA = actionMap.FindAction("HLA");
        LHA = actionMap.FindAction("LHA");
        LLA = actionMap.FindAction("LLA");

        // Enable input actions
        actionMap.Enable();
        moveAction.Enable();
        HHA.Enable();
        HLA.Enable();
        LHA.Enable();
        LLA.Enable();

        // Subscribe to input performed/canceled events
        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCancelled;
        HHA.performed += OnHHA;
        HHA.canceled += OnHHACancelled;
        HLA.performed += OnHLA;
        HLA.canceled += OnHLACancelled;
        LHA.performed += OnLHA;
        LHA.canceled += OnLHACancelled;
        LLA.performed += OnLLA; // Corrected from OnLLACancelled to OnLLA
        LLA.canceled += OnLLACancelled;


        // Get the player's main collider
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("PlayerMovement: No Collider2D found on this GameObject. A collider is required for collision detection.");
        }
    }

    private void OnDisable()
    {
        // Disable input actions and unsubscribe events
        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMoveCancelled;
        moveAction.Disable();
        HHA.performed -= OnHHA;
        HHA.canceled -= OnHHACancelled;
        HHA.Disable();
        HLA.performed -= OnHLA;
        HLA.canceled -= OnHLACancelled;
        HLA.Disable();
        LHA.performed -= OnLHA;
        LHA.canceled -= OnLHACancelled;
        LHA.Disable();
        LLA.performed -= OnLLA;
        LLA.canceled -= OnLLACancelled;
        LLA.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        // Read movement input from the player when performed
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
        // Reset movement when input is cancelled
        moveInput = Vector2.zero;
    }

    private void OnHHA(InputAction.CallbackContext context)
    {
        if (HHB.isInRange && cd <= 0)
        {
            HHB.Dummy.GetComponent<DummyStats>().DecrementHP(2 * Strength);
            cd = 0.25f;
        }
    }

    private void OnHHACancelled(InputAction.CallbackContext context)
    {
        // Optional: Add logic if releasing HHA does something
    }

    private void OnHLA(InputAction.CallbackContext context)
    {
        if (HHB.isInRange && cd <= 0)
        {
            HHB.Dummy.GetComponent<DummyStats>().DecrementHP(Strength);
            cd = 0.25f;
        }
    }

    private void OnHLACancelled(InputAction.CallbackContext context)
    {
        // Optional: Add logic if releasing HLA does something
    }

    private void OnLHA(InputAction.CallbackContext context)
    {
        if (LHB.isInRange && cd <= 0)
        {
            LHB.Dummy.GetComponent<DummyStats>().DecrementHP(2 * Strength);
            cd = 0.25f;
        }
    }

    private void OnLHACancelled(InputAction.CallbackContext context)
    {
        // Optional: Add logic if releasing LHA does something
    }

    private void OnLLA(InputAction.CallbackContext context)
    {
        if (LHB.isInRange && cd <= 0)
        {
            LHB.Dummy.GetComponent<DummyStats>().DecrementHP(Strength);
            cd = 0.25f;
        }
    }

    private void OnLLACancelled(InputAction.CallbackContext context)
    {
        // Optional: Add logic if releasing LLA does something
    }

    private void Update()
    {
        // Manual Gravity
        if (!Grounded)
        {
            currentVelocity.y += gravity * Time.deltaTime;
        }
        else if (currentVelocity.y < 0) // Reset vertical velocity if grounded and moving downwards
        {
            currentVelocity.y = 0f; // Set to 0 to prevent downward movement into the ground
        }

        // --- Horizontal Movement with Collision Prediction ---
        float targetXVelocity = moveInput.x * moveSpeed;
        float moveAmountX = targetXVelocity * Time.deltaTime; // The distance we want to move this frame

        // Perform a BoxCast (or CapsuleCast depending on your player's collider shape)
        // in the direction of horizontal movement
        RaycastHit2D hit = new RaycastHit2D();
        
        // Ensure playerCollider is not null before using it
        if (playerCollider == null)
        {
            currentVelocity.x = 0; // Prevent movement if collider is missing
            Debug.LogError("PlayerMovement: playerCollider is null. Cannot perform BoxCast for horizontal collision.");
        }
        else
        {
            Vector2 origin = playerCollider.bounds.center; // Center of the player's collider
            // Use playerCollider.bounds.size directly for the BoxCast
            // Make size slightly smaller than actual collider to prevent sticking (e.g., 90% of actual size)
            Vector2 size = playerCollider.bounds.size * 0.9f;
            
            float distance = Mathf.Abs(moveAmountX); // Distance to check is the magnitude of desired movement
            Vector2 direction = Vector2.right * Mathf.Sign(moveAmountX); // Direction is left or right

            // Only cast if there's horizontal movement intended
            if (moveAmountX != 0)
            {
                hit = Physics2D.BoxCast(
                    origin,
                    size,
                    0f, // Angle
                    direction,
                    distance,
                    groundLayer // Check against groundLayer (which should include walls)
                );
            }

            if (hit.collider != null && hit.distance > 0f) // If we hit something and it's not the player's own collider
            {
                // We hit a wall, so stop horizontal movement
                currentVelocity.x = 0f;
                // Optional: Snap player right up to the wall to prevent gaps
                // transform.position = hit.point - direction * (playerCollider.bounds.extents.x + 0.001f); 
                // This snapping can be complex to get right without jitter, often better to just stop movement.
            }
            else
            {
                // No collision in the way, so apply the desired horizontal velocity
                currentVelocity.x = targetXVelocity;
            }
        }
        // --- End Horizontal Movement with Collision Prediction ---


        // Jump
        // Check for jump input and if grounded
        // We use moveInput.y > 0.1f to allow for a slight joystick push up, adjust as needed for button input
        if (moveInput.y > 0.1f && Grounded) 
        {
            currentVelocity.y = jumpForce;
            Grounded = false; // Immediately set to false to allow falling after jump
        }

        // Apply movement
        transform.Translate(currentVelocity * Time.deltaTime);

        // Grounded and Wall Checks (OverlapCircle still used for status, not for stopping movement)
        // These are for 'status' flags (e.g., for animations, or enabling wall jump)
        // The actual prevention of movement is handled by the BoxCast above.
        Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        againstLeftWall = Physics2D.OverlapCircle(leftWallCheck.position, groundCheckRadius, groundLayer);
        againstRightWall = Physics2D.OverlapCircle(rightWallCheck.position, groundCheckRadius, groundLayer);


        Debug.Log(moveInput); // Still useful for debugging input
        
        cd -= Time.deltaTime;
        selectionTimer -= Time.deltaTime;

        if (selectionTimer <= 0)
        {
            button1.SetActive(true);
            button2.SetActive(true);
            button3.SetActive(true);
        }
    }

    // Optional: Draw gizmos to visualize the check areas in the editor
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        if (leftWallCheck != null)
        {
            Gizmos.color = Color.cyan; // Different color for wall checks
            Gizmos.DrawWireSphere(leftWallCheck.position, groundCheckRadius);
        }
        if (rightWallCheck != null)
        {
            Gizmos.color = Color.cyan; // Different color for wall checks
            Gizmos.DrawWireSphere(rightWallCheck.position, groundCheckRadius);
        }

        // Draw a gizmo for the BoxCast area
        if (playerCollider != null)
        {
            Gizmos.color = Color.blue;
            // Draw a wire cube representing the bounds used for the BoxCast
            Gizmos.DrawWireCube(playerCollider.bounds.center, playerCollider.bounds.size * 0.9f);
        }
    }

    public void IncreaseSpeed()
    {
        moveSpeed += 0.5f; // Adjust increment as moveSpeed is now meters/second
        selectionTimer = 45;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }

    public void IncreaseStrength()
    {
        Strength += 2;
        selectionTimer = 45;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }

    public void IncreaseJump()
    {
        jumpForce += 1f; // Adjust increment for jumpForce
        selectionTimer = 45;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }
}