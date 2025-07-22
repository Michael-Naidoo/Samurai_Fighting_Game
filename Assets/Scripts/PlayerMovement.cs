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
    public Transform groundCheck; // An empty GameObject child at the player's feet
    public float groundCheckRadius = 0.2f; // Radius for ground detection

    [SerializeField] private bool Grounded;

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
        LLA.performed += OnLLACancelled; // Typo corrected from OnLLACancelled to OnLLA and then OnLLACancelled
        LLA.performed += OnLLA;

        // No longer needed: rb2D = gameObject.GetComponent<Rigidbody2D>();
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
        LLA.performed -= OnLLA; // Corrected
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
            currentVelocity.y = 0f; // 0 value to keep it "stuck" to the ground
        }

        // Horizontal Movement
        currentVelocity.x = moveInput.x * moveSpeed;

        // Jump
        if (moveInput.y > 0.1f && Grounded) // Check for jump input and if grounded
        {
            currentVelocity.y = jumpForce;
            Grounded = false; // Immediately set to false to allow falling
        }

        // Apply movement
        transform.Translate(currentVelocity * Time.deltaTime);

        // Grounded Check (Raycast or OverlapCircle)
        // Using OverlapCircle for 2D character ground check
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

    // No longer needed: OnCollisionEnter2D and OnCollisionExit2D
    // You handle ground checks manually now.

    // Optional: Draw a gizmo to visualize the ground check area in the editor
    private void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
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