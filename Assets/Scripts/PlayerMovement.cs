using System;
using System.Collections;
using DefaultNamespace;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using Object = UnityEngine.Object;

public class PlayerMovement : MonoBehaviour
{
    private PlayerCharacter playerCharacter;
    
    // No longer needed: private Rigidbody2D rb2D;
    public InputActionAsset inputActions;
    
    private InputAction moveAction;
    private InputAction HHA;
    private InputAction HLA;
    private InputAction LHA;
    private InputAction LLA;
    private Vector2 moveInput;
    private Animator animator;
    

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
    [SerializeField] private Vector2 attackDirection;
    public float attackDistance;
    public float weaponDamage;
    [SerializeField] private LayerMask player1Layer;
    [SerializeField] private LayerMask player2Layer;
    private Collider2D player;
    public float currentCooldown = 0.25f;
    public float startUpTime = 5;

    public float staminaGage;
    public Slider staminaSlider;


    private void Awake()
    {
        playerCollider = gameObject.GetComponent<Collider2D>();
        animator = GetComponentInChildren<Animator>();
        staminaRecoveryRate = 10;
        
        if (gameObject.CompareTag("Player1"))
        {
            staminaSlider = GameObject.FindWithTag("P1Stamina").GetComponent<Slider>();
        }
        else if (gameObject.CompareTag("Player2"))
        {
            staminaSlider = GameObject.FindWithTag("P2Stamina").GetComponent<Slider>();
        }
        playerCharacter = GetComponent<PlayerCharacter>(); // NEW: Cache the component
    }

    public void OnSwapWeapon(InputAction.CallbackContext context)
    {
        gameObject.GetComponent<WeaponsHandler>().SwitchWeapon();
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
        if (context.canceled)
        {
            animator.SetFloat("AnimState", 0);   
        }
        else
        {
            animator.SetFloat("AnimState", 7);
        }
    }

    public void OnMoveCancelled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }
    float drawTime = 0;
    float difference = 0;
    public GameObject arrowPrefab;
    [SerializeField] private float staminaRecoveryRate;

    public void OnHHA(InputAction.CallbackContext context)
    {
        var weaponsHandler = GetComponent<WeaponsHandler>();
        if (context.started)
        {
            if ((weaponsHandler.usingPrimaryWeapon && weaponsHandler.primaryWeapon == WeaponsHandler.Weapons.bow) ||
                (!weaponsHandler.usingPrimaryWeapon && weaponsHandler.secondaryWeapon == WeaponsHandler.Weapons.bow))
            {
                difference = 0;
                drawTime = Time.time;
                if (cd <= 0)
                {
                    animator.SetInteger("AnimState", 3);
                }
            }
            else if (staminaGage > 20 * playerCharacter.FinalStamina)
            {
                if (cd <= 0)
                {
                    animator.SetInteger("AnimState", 5);
                }
                Debug.DrawLine(HHB.position,
                    new Vector3((HHB.position.x + attackDistance) * attackDirection.x, HHB.position.y));
                if (gameObject.CompareTag("Player1"))
                {
                    Vector2 boxCenter = (Vector2)HHB.position + new Vector2(attackDistance / 2 * attackDirection.x, 0);
                    player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player2Layer);
                }
                else if (gameObject.CompareTag("Player2"))
                {
                    Vector2 boxCenter = (Vector2)HHB.position + new Vector2(attackDistance / 2 * attackDirection.x, 0);
                    player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player1Layer);
                }

                if (player && cd <= 0)
                {
                    Debug.Log(playerCharacter.FinalStrength * weaponDamage);
                    StartCoroutine(WaitHigh(startUpTime));
                    cd = currentCooldown;
                }
                staminaGage -= 20 * playerCharacter.FinalStamina;
            }

            
        }
        else if(context.canceled && staminaGage > 35 * playerCharacter.FinalStamina)
        {
            animator.SetInteger("AnimState", 0);
            if ((weaponsHandler.usingPrimaryWeapon && weaponsHandler.primaryWeapon == WeaponsHandler.Weapons.bow) ||
                (!weaponsHandler.usingPrimaryWeapon && weaponsHandler.secondaryWeapon == WeaponsHandler.Weapons.bow))
            {
                Debug.Log("working till this point");
                var releaseTime = Time.time;
                difference = Mathf.Clamp(releaseTime - drawTime, 0.5f, 1.3f);
                GameObject arrow = Instantiate(arrowPrefab, transform.position, quaternion.identity, GameObject.FindWithTag("MainCamera").transform);
                Debug.Log(arrow);
                var arrowScript = arrow.GetComponent<ArrowScript>();
                arrowScript.initialTime = Time.time;
                arrowScript.gravity = -0.5f;
                arrowScript.multiplyer = 0.2f;
                arrowScript.InitialVelocityMagnitude = difference;
                arrowScript.angle = attackDirection.x;
                arrowScript.initialPosition = transform.position;
                arrowScript.damage = weaponDamage * playerCharacter.FinalStrength;
                arrowScript.thisPlayer = gameObject.GetComponent<Collider2D>();
                arrowScript.speed = 15;
            }

            staminaGage -= 35 * playerCharacter.FinalStamina;
        }
    }

    private void DrawBow()
    {
        //start timer
    }

    public void OnHHACancelled(InputAction.CallbackContext context)
    {
    }

    public void OnHLA(InputAction.CallbackContext context)
    {
        animator.SetInteger("AnimState", 1);
        gameObject.GetComponent<DummyStats>().HighParry();
        cd = 0.2f;
        animator.SetInteger("AnimState", 0);
    }

    public void OnHLACancelled(InputAction.CallbackContext context)
    {
    }

    public void OnLHA(InputAction.CallbackContext context)
    {
        var weaponsHandler = GetComponent<WeaponsHandler>();
        if (context.started)
        {
            
            if ((weaponsHandler.usingPrimaryWeapon && weaponsHandler.primaryWeapon == WeaponsHandler.Weapons.bow) ||
                (!weaponsHandler.usingPrimaryWeapon && weaponsHandler.secondaryWeapon == WeaponsHandler.Weapons.bow))
            {
                difference = 0;
                drawTime = Time.time;
                if (cd <= 0)
                {
                    animator.SetInteger("AnimState", 4);
                }
            }

            else if (staminaGage > 25 * playerCharacter.FinalStamina)
            {
                if (cd <= 0)
                {
                    animator.SetInteger("AnimState", 6);
                }
                Debug.DrawLine(LHB.position,
                    new Vector3((LHB.position.x + attackDistance) * attackDirection.x, LHB.position.y));
                if (gameObject.CompareTag("Player1"))
                {
                    Vector2 boxCenter = (Vector2)LHB.position + new Vector2(attackDistance / 2 * attackDirection.x, 0);
                    player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player2Layer);
                }
                else if (gameObject.CompareTag("Player2"))
                {
                    Vector2 boxCenter = (Vector2)LHB.position + new Vector2(attackDistance / 2 * attackDirection.x, 0);
                    player = Physics2D.OverlapBox(boxCenter, new Vector2(attackDistance, 0), 0, player1Layer);
                }
                if (player && cd <= 0)
                {
                    StartCoroutine(WaitLow(startUpTime));
            
                    cd = currentCooldown;
                }
                staminaGage -= 25 * playerCharacter.FinalStamina;
            }
        }
        else if(context.canceled && staminaGage > 10 * playerCharacter.FinalStamina)
        {
            animator.SetInteger("AnimState", 0);
            if ((weaponsHandler.usingPrimaryWeapon && weaponsHandler.primaryWeapon == WeaponsHandler.Weapons.bow) ||
                (!weaponsHandler.usingPrimaryWeapon && weaponsHandler.secondaryWeapon == WeaponsHandler.Weapons.bow))
            {
                var releaseTime = Time.time;
                difference = Mathf.Clamp(releaseTime - drawTime, 0, 3);
                GameObject arrow = Instantiate(arrowPrefab, transform.position, quaternion.identity, GameObject.FindWithTag("MainCamera").transform);
                var arrowScript = arrow.GetComponent<ArrowScript>();
                arrowScript.initialTime = Time.time;
                arrowScript.gravity = 0;
                arrowScript.InitialVelocityMagnitude = difference;
                arrowScript.angle = attackDirection.x;
                arrowScript.initialPosition = transform.position;
                arrowScript.damage = weaponDamage * playerCharacter.FinalStrength;
                arrowScript.thisPlayer = gameObject.GetComponent<Collider2D>();
                arrowScript.speed = 20;
            }
            staminaGage -= 10 * playerCharacter.FinalStamina;
        }
    }

    public void OnLHACancelled(InputAction.CallbackContext context)
    {
    }

    public void OnLLA(InputAction.CallbackContext context)
    {
        Debug.Log(context);
        animator.SetInteger("AnimState", 1);
        gameObject.GetComponent<DummyStats>().LowParry();
        cd = 0.2f;
        animator.SetInteger("AnimState", 0);
    }

    public void OnLLACancelled(InputAction.CallbackContext context)
    {
    }

    public void Update()
    {
        staminaSlider.value = staminaGage;
        if (staminaGage < 100)
        {
            staminaGage += (100 * (staminaRecoveryRate / 60)) * Time.deltaTime;
        }
        else if (staminaGage > 100)
        {
            staminaGage = 100;
        }
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
        float targetXVelocity = moveInput.x * playerCharacter.FinalMoveSpeed;
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
                Vector2 rayOriginLeft =
                    new Vector2(transform.position.x - playerHalfWidth, playerCollider.bounds.center.y);

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
                        if (currentVelocity.x > 0)
                            currentVelocity.x = 0; // Prevent pushing backwards if already past hit point
                    }

                    againstLeftWall = actualMoveDistance <= 0.05f; // Set status if very close to wall
                }
                else
                {
                    againstLeftWall = false; // Not against wall if no hit
                }
            }
            else if (moveInput.x > 0) // Moving Right
            {
                // Raycast origin from the right edge of the player
                Vector2 rayOriginRight =
                    new Vector2(transform.position.x + playerHalfWidth, playerCollider.bounds.center.y);

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
                }
                else
                {
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
            currentVelocity.y = playerCharacter.FinalJumpForce;
            Grounded = false;
        }

        // Apply movement
        transform.Translate(currentVelocity * Time.deltaTime);

        // Grounded Check
        Grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);


        Debug.Log(moveInput);

        cd -= Time.deltaTime;
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
        //moveSpeed += 0.5f;
    }

    public void IncreaseStrength()
    {
       // Strength += 2;
    }

    public void IncreaseJump()
    {
       // jumpForce += 1f;
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
    
    IEnumerator WaitLow(float time)
    {
        yield return new WaitForSeconds(time);
        player.GetComponent<DummyStats>().DecrementHP(playerCharacter.FinalStrength * weaponDamage, DummyStats.AttackType.Low);
    }
    IEnumerator WaitHigh(float time)
    {
        yield return new WaitForSeconds(time);
        player.GetComponent<DummyStats>().DecrementHP(playerCharacter.FinalStrength * weaponDamage, DummyStats.AttackType.High);
    }
}