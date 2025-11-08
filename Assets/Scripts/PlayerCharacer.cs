// MODIFIED SCRIPT: PlayerCharacter.cs (Attach to Player GameObjects)
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    public int playerIndex; // 0 for Player 1, 1 for Player 2
    
    [Header("TUM Buff Management")]
    private BuffData storedBuff; // The buff chosen (but not yet active)
    public bool isBuffActive = false; // Flag for reset logic
    

    [Header("Buff Multipliers (Applied during activation)")]
    private float strengthMultiplier = 1.0f;
    private float speedMultiplier = 1.0f;
    private float jumpMultiplier = 1.0f;
    private float staminaMultiplier = 1.0f;
    
    // --- Public Properties for PlayerMovement to READ ---
    // PlayerMovement MUST read these instead of its own internal variables.
    
    [Header("Base Stats (Read by PlayerMovement)")]
    [SerializeField] private float baseStrength = 5f; // Initial value from PlayerMovement
    [SerializeField] private float baseMoveSpeed = 5f; // Initial value from PlayerMovement
    [SerializeField] private float baseJumpForce = 8f; // Initial value from PlayerMovement
    
    // Calculated Final Values
    public float FinalStrength => baseStrength * strengthMultiplier;
    public float FinalMoveSpeed => baseMoveSpeed * speedMultiplier;
    public float FinalJumpForce => baseJumpForce * jumpMultiplier;

    public float FinalStamina => staminaMultiplier;

    // Start/Awake for setup
    private void Awake()
    {
        // Optional: Mirror initial stats from PlayerMovement on start
        // PlayerMovement movement = GetComponent<PlayerMovement>();
        // if (movement != null)
        // {
        //     baseStrength = movement.Strength;
        //     baseMoveSpeed = movement.moveSpeed;
        //     baseJumpForce = movement.jumpForce;
        // }
    }

    public void OnTUM_Activate(InputValue value)
    {
        // Check if the input was pressed this frame
        if (value.isPressed)
        {
            // --- B. Buff Activation (Tactical Trigger) ---
            if (storedBuff != null && !isBuffActive)
            {
                ApplyBuffEffect(storedBuff); // Calls the method to set the multiplier
            }
        }
    }
    public void TUM_Activate()
    {
        // Check if the input was pressed this frame
        // --- B. Buff Activation (Tactical Trigger) ---
            if (storedBuff != null && !isBuffActive)
            {
                Debug.Log(storedBuff);
                var buff = storedBuff;
                ApplyBuffEffect(buff); // Calls the method to set the multiplier
            }
    }

    // Called by GameManager after selection
    public void StoreBuff(BuffData newBuff)
    {
        storedBuff = newBuff;
        // TODO: Update UI to show the player has a buff ready
    }

    private void ApplyBuffEffect(BuffData buff)
    {
        isBuffActive = true;
        
        // Reset all multipliers to base 1.0 before applying the new one
        ResetBuff();
        
        //ResetMultipliers();
        Debug.Log(buff);
        
        float value = 1 + buff.effectValue; // e.g., 1.15 for 15% boost

        switch (buff.effectType)
        {
            case BuffData.BuffEffectType.Strength:
                strengthMultiplier = value;
                break;
                
            case BuffData.BuffEffectType.Speed:
                speedMultiplier = value;
                break;
                
            case BuffData.BuffEffectType.Jump:
                jumpMultiplier = value;
                break;
            case BuffData.BuffEffectType.StaminaCostReduction:
                staminaMultiplier = value;
                break;
        }
        
        //Debug.Log($"P{playerIndex + 1} activated {storedBuff.buffName}. Strength: {FinalStrength}");
        // 
    }

    // --- C. Buff Reset Logic ---
    // Called by GameManager on Stock Loss or Round End.
    public void ResetBuff()
    {
        // 1. Remove the temporary effect (reverse the multiplier)
        if (isBuffActive)
        {
            ResetMultipliers();
        }
        
        // 2. Clear the storage so a new buff can be offered
        storedBuff = null;
        isBuffActive = false;
        
        Debug.Log($"P{playerIndex + 1} buff state reset. Stats returned to base values.");
    }
    
    private void ResetMultipliers()
    {
        strengthMultiplier = 1.0f;
        speedMultiplier = 1.0f;
        jumpMultiplier = 1.0f;
        staminaMultiplier = 1.0f;
    }
}