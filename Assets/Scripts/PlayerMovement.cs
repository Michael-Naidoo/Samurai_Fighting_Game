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
    // These `leftWallCheck` and `rightWallCheck` transforms can still be useful for visually setting
    // the general area for wall checks, but the raycast origins will be more precise.
    public Transform leftWallCheck; 
    public Transform rightWallCheck; 
    public float groundCheckRadius = 0.2f; // Radius for ground detection (also used for wall checks for simplicity)

    // Reference to the player's main collider for BoxCast/CapsuleCast
    private Collider2D playerCollider;

    [SerializeField] private bool Grounded;
    [SerializeField] private bool againstLeftWall;
    [SerializeField] private bool againstRightWall;
    [SerializeField] private RaycastHit2D approachingLeftWall;
    [SerializeField] private RaycastHit2D approachingRightWall;
    // Small offset to ensure the ray starts just outside the player's collider
    public float raycastOffset = 0.01f; // A very small value, adjust if needed

    public Transform HHB;
    public Transform LHB;
    private float cd;

    public GameObject button1;
    public GameObject button2;
    public GameObject button3;
    public float selectionTimer;
    [SerializeField]private Vector2 attackDirection;
    [SerializeField]private float attackDistance;
    [SerializeField]private LayerMask player1Layer;
    [SerializeField]private LayerMask player2Layer;
    private Collider2D player;

    public void OnEnable()
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
        LLA.performed += OnLLA; 
        LLA.canceled += OnLLACancelled;
        
        // Get the player's main collider
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError(
                "PlayerMovement: No Collider2D found on this GameObject. A collider is required for collision detection.");
        }
    }

    public void OnDisable()
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

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        if (gameObject.CompareTag("Player1"))
        {
            GameObject other = GameObject.FindWithTag("Player2");
            CalculateDirection(other);
        }
        else
        {
            if (gameObject.CompareTag("Player2"))
            {
                GameObject other = GameObject.FindWithTag("Player1");
                CalculateDirection(other);
            }
        }
    }

    public void OnMoveCancelled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    public void OnHHA(InputAction.CallbackContext context)
    {
        Debug.DrawLine(HHB.position, new Vector3((HHB.position.x + attackDistance) * attackDirection.x, HHB.position.y));
        if (gameObject.CompareTag("Player1"))
        {
            Debug.Log(context);
            Vector2 boxCenter = (Vector2)HHB.position + new Vector2(attackDistance/2 * attackDirection.x, 0);
            player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player2Layer);
        }
        else if (gameObject.CompareTag("Player2"))
        {
            Vector2 boxCenter = (Vector2)HHB.position + new Vector2(attackDistance/2 * attackDirection.x, 0);
            player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player1Layer);
        }
        
        if (player && cd <= 0)
        {
            player.GetComponent<DummyStats>().DecrementHP(Strength);
            cd = 0.25f;
        }
    }

    public void OnHHACancelled(InputAction.CallbackContext context) {}

    public void OnHLA(InputAction.CallbackContext context)
    {
        Debug.Log(context);
       // if (HHB.isInRange && cd <= 0)
        {
          //  HHB.Dummy.GetComponent<DummyStats>().DecrementHP(Strength);
            cd = 0.25f;
        }
    }

    public void OnHLACancelled(InputAction.CallbackContext context) {}

    public void OnLHA(InputAction.CallbackContext context)
    {
        Debug.DrawLine(LHB.position, new Vector3((LHB.position.x + attackDistance) * attackDirection.x, LHB.position.y));
        if (gameObject.CompareTag("Player1"))
        {
            Debug.Log(context);
            Vector2 boxCenter = (Vector2)LHB.position + new Vector2(attackDistance/2 * attackDirection.x, 0);
            player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player2Layer);
        }
        else if (gameObject.CompareTag("Player2"))
        {
            Vector2 boxCenter = (Vector2)LHB.position + new Vector2(attackDistance/2 * attackDirection.x, 0);
            player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player2Layer);
        }
        
        if (player && cd <= 0)
        {
            player.GetComponent<DummyStats>().DecrementHP(Strength);
            cd = 0.25f;
        }
    }

    public void OnLHACancelled(InputAction.CallbackContext context) {}

    public void OnLLA(InputAction.CallbackContext context)
    {
        Debug.Log(context);
       // if (LHB.isInRange && cd <= 0)
        {
           // LHB.Dummy.GetComponent<DummyStats>().DecrementHP(Strength);
            cd = 0.25f;
        }
    }

    public void OnLLACancelled(InputAction.CallbackContext context) {}

    public void Update()
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
        float moveDistanceThisFrame = targetXVelocity * Time.deltaTime; 

        // Initialize raycast hits for the current frame
        approachingLeftWall = new RaycastHit2D();
        approachingRightWall = new RaycastHit2D();

        // Calculate player's half-width for accurate raycast origin and hit point calculation
        float playerHalfWidth = playerCollider.bounds.extents.x;

        // Determine the maximum distance the ray should check
        // It should be at least enough to cover the current frame's intended movement
        // plus the player's half-width to project from the center to the edge, plus a small buffer.
        float maxRayDistance = Mathf.Abs(moveDistanceThisFrame) + raycastOffset;
        
        // Default horizontal velocity to target, will be clamped if a wall is hit
        currentVelocity.x = targetXVelocity;

        if (playerCollider != null) // Ensure collider exists before casting rays
        {
            if (moveInput.x < 0) // Moving Left
            {
                // Raycast origin from the left edge of the player
                Vector2 rayOriginLeft = new Vector2(transform.position.x - playerHalfWidth, playerCollider.bounds.center.y);
                
                approachingLeftWall = Physics2D.Raycast(rayOriginLeft, Vector2.left, maxRayDistance, groundLayer);

                // Visualize the raycast for debugging in Scene view
                Debug.DrawRay(rayOriginLeft, Vector2.left * maxRayDistance, Color.red);

                if (approachingLeftWall.collider != null) // If a wall is detected
                {
                    // Calculate the actual distance the player's edge can move
                    float actualMoveDistance = approachingLeftWall.distance - playerHalfWidth - raycastOffset;
                    
                    // Clamp the horizontal velocity if we're about to hit a wall
                    if (actualMoveDistance < Mathf.Abs(moveDistanceThisFrame))
                    {
                        currentVelocity.x = -actualMoveDistance / Time.deltaTime;
                        if (currentVelocity.x > 0) currentVelocity.x = 0; // Prevent pushing backwards if already past hit point
                    }
                    againstLeftWall = actualMoveDistance <= 0.05f; // Set status if very close to wall
                } else {
                    againstLeftWall = false; // Not against wall if no hit
                }
            }
            else if (moveInput.x > 0) // Moving Right
            {
                // Raycast origin from the right edge of the player
                Vector2 rayOriginRight = new Vector2(transform.position.x + playerHalfWidth, playerCollider.bounds.center.y);

                approachingRightWall = Physics2D.Raycast(rayOriginRight, Vector2.right, maxRayDistance, groundLayer);

                // Visualize the raycast for debugging in Scene view
                Debug.DrawRay(rayOriginRight, Vector2.right * maxRayDistance, Color.red);

                if (approachingRightWall.collider != null) // If a wall is detected
                {
                    float actualMoveDistance = approachingRightWall.distance - playerHalfWidth - raycastOffset;

                    if (actualMoveDistance < moveDistanceThisFrame)
                    {
                        currentVelocity.x = actualMoveDistance / Time.deltaTime;
                        if (currentVelocity.x < 0) currentVelocity.x = 0; // Prevent pushing backwards
                    }
                    againstRightWall = actualMoveDistance <= 0.05f; // Set status if very close to wall
                } else {
                    againstRightWall = false; // Not against wall if no hit
                }
            }
            else // No horizontal input
            {
                currentVelocity.x = 0f;
                againstLeftWall = false; // Reset status
                againstRightWall = false; // Reset status
            }
        }
        else // Fallback if playerCollider is null
        {
            currentVelocity.x = 0f;
            againstLeftWall = false;
            againstRightWall = false;
        }
        // --- End Horizontal Movement with Collision Prediction ---


        // Jump
        if (moveInput.y > 0.1f && Grounded) 
        {
            currentVelocity.y = jumpForce;
            Grounded = false; 
        }

        // Apply movement
        transform.Translate(currentVelocity * Time.deltaTime);

        // Grounded Check
        Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        Debug.Log(moveInput); 
        
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
    public void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
        
        if (player)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, player.transform.position);
        }
        
        // These are just for general reference if you want specific points
        if (leftWallCheck != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(leftWallCheck.position, groundCheckRadius);
        }
        if (rightWallCheck != null)
        {
            Gizmos.color = Color.cyan; 
            Gizmos.DrawWireSphere(rightWallCheck.position, groundCheckRadius);
        }

        if (playerCollider != null)
        {
            Gizmos.color = Color.blue;
            // Draw a wire cube representing the player's collider bounds
            Gizmos.DrawWireCube(playerCollider.bounds.center, playerCollider.bounds.size);
            
            // Visualize the actual raycasts for horizontal movement
            float playerHalfWidth = playerCollider.bounds.extents.x;
            float raycastOffset = 0.01f;
            // Use the current velocity (or target velocity for a full expected ray) for gizmo length
            float maxGizmoRayDistance = Mathf.Abs(currentVelocity.x * Time.deltaTime) + playerHalfWidth + raycastOffset; 
            // Clamp minimum length so you always see it
            if (maxGizmoRayDistance < 0.1f) maxGizmoRayDistance = 0.1f; 

            if (moveInput.x < 0) // If player is trying to move Left
            {
                Vector2 rayOrigin = new Vector2(transform.position.x - playerHalfWidth, playerCollider.bounds.center.y);
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.left * maxGizmoRayDistance);
                if (approachingLeftWall.collider != null)
                {
                    Gizmos.DrawWireSphere(approachingLeftWall.point, 0.05f); // Hit point
                }
            }
            else if (moveInput.x > 0) // If player is trying to move Right
            {
                Vector2 rayOrigin = new Vector2(transform.position.x + playerHalfWidth, playerCollider.bounds.center.y);
                Gizmos.color = Color.magenta;
                Gizmos.DrawLine(rayOrigin, rayOrigin + Vector2.right * maxGizmoRayDistance);
                if (approachingRightWall.collider != null)
                {
                    Gizmos.DrawWireSphere(approachingRightWall.point, 0.05f); // Hit point
                }
            }
        }
    }

    public void IncreaseSpeed()
    {
        moveSpeed += 0.5f; 
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
        jumpForce += 1f;
        selectionTimer = 45;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }
    
    public void CalculateDirection(GameObject other)
    {
        if (gameObject.transform.position.x <= other.transform.position.x)
        {
            attackDirection.x = 1;
        }
        else if (gameObject.transform.position.x > other.transform.position.x)
        {
            attackDirection.x = -1;
        }
    }
}