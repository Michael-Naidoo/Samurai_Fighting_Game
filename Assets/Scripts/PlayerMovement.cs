using System;
using DefaultNamespace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public InputActionAsset inputActions;
    public float moveSpeed = 500f;
    public float jumpForce = 250F;
    public float Strength = 5f;
    private InputAction moveAction;
    private InputAction HHA;
    private InputAction HLA;
    private InputAction LHA;
    private InputAction LLA;
    private Vector2 moveInput;
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
        LLA.performed += OnLLA;
        LLA.canceled += OnLLACancelled;

        rb2D = gameObject.GetComponent<Rigidbody2D>();
    }
    private void OnDisable()
    {
// Disable input actions and unsubscribe events
        moveAction.performed -= OnMove; // This listens for when the player presses a movement key like a or d
        moveAction.canceled -= OnMoveCancelled; // This listens for when the player releases the key
        moveAction.Disable(); // It turns off the input action - Unity will stop listening for that specific input. Improves performance by stopping unnecessary input checks.
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
// Read movement input from the player when performed ^
        moveInput = context.ReadValue<Vector2>(); 
    }
    private void OnMoveCancelled(InputAction.CallbackContext context)
    {
// Reset movement when input is cancelled ^
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
        
    }
    
    private void Update()
    {
// Apply movement based on input
        Vector2 movementHor = new Vector2(moveInput.x, 0) * moveSpeed *
                           Time.deltaTime;
        Vector2 movementVert = new Vector2(0, moveInput.y) * jumpForce *
                           Time.deltaTime;
        rb2D.AddForce(movementHor, ForceMode2D.Force);
        if (Grounded)
        {
            rb2D.AddForce(movementVert, ForceMode2D.Impulse);
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            Grounded = false;
        }
    }

    public void IncreaseSpeed()
    {
        moveSpeed += 50;
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
        jumpForce += 50;
        selectionTimer = 45;
        button1.SetActive(false);
        button2.SetActive(false);
        button3.SetActive(false);
    }
}
