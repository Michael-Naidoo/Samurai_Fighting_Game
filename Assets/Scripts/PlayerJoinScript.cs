using DefaultNamespace;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerJoinScript : MonoBehaviour
{
    // --- Public/Serialized Fields (Reverted to separate prefabs) ---
    public Transform spawnPoint1, spawnPoint2;
    public GameObject player1Prefab, player2Prefab; // The base Player 1 and Player 2 prefabs
    
    [SerializeField] private GameObject[] characters;
    
    // --- Private/Internal Fields ---
    // These will hold the INSTANTIATED objects correctly mapped to their roles
    private GameObject realPlayer1, realPlayer2; 
    private GameObject player1Character, player2Character;
    private PlayerDataManager playerDataManager;
    
    public Slider player1HealthSlider; 
    public Slider player2HealthSlider;
    public Slider player1StaminaSlider;
    public Slider player2StaminaSlider;
    
    // Helper class to store temporary data
    private class PlayerSpawn
    {
        public int controllerId;
        public GameObject playerObject;
        public int desiredPlayerIndex; // 1 for P1Prefab, 2 for P2Prefab
    }

    private void Awake()
    {
        // 1. Initialize PlayerDataManager
        playerDataManager = PlayerDataManager.Instance;
        if (playerDataManager == null)
        {
            Debug.LogError("PlayerDataManager not found. Cannot initialize players.");
            return;
        }

        // 2. Instantiate Base Player Objects (P1 Prefab at Spawn1, P2 Prefab at Spawn2)
        GameObject tempP1Obj = Instantiate(player1Prefab, spawnPoint1.position, spawnPoint1.rotation);
        GameObject tempP2Obj = Instantiate(player2Prefab, spawnPoint2.position, spawnPoint2.rotation);

        // 3. Get Controller IDs and package data
        PlayerSpawn data1 = GetPlayerSpawnData(tempP1Obj, 1);
        PlayerSpawn data2 = GetPlayerSpawnData(tempP2Obj, 2);

        if (data1 == null || data2 == null)
        {
            Debug.LogError("Could not get Gamepad data for both player objects.");
            return;
        }
        
        Debug.Log($"P1 Index (Saved): {playerDataManager.player1Index}, P2 Index (Saved): {playerDataManager.player2Index}");
        Debug.Log($"Temp P1 Obj ID: {data1.controllerId}, Temp P2 Obj ID: {data2.controllerId}");

        // 4. Map the instantiated objects to their final roles (realPlayer1/realPlayer2)
        
        // Scenario 1: P1 Prefab (data1) grabbed the P1 controller ID
        if (data1.controllerId == playerDataManager.player1Index)
        {
            realPlayer1 = data1.playerObject; // P1Prefab is the Real P1
            realPlayer2 = data2.playerObject; // P2Prefab is the Real P2
            
            
        }
        // Scenario 2: P1 Prefab (data1) grabbed the P2 controller ID
        else if (data1.controllerId == playerDataManager.player2Index)
        {
            realPlayer1 = data2.playerObject; // P2Prefab must be the Real P1
            realPlayer2 = data1.playerObject; // P1Prefab is the Real P2
        }
        else
        {
            // Fallback/Error case (e.g., mismatched IDs, only one controller)
            Debug.LogError("Controller IDs do not match saved Player Indices. Using default assignment (TempP1Obj=RealP1, TempP2Obj=RealP2).");
            realPlayer1 = data1.playerObject;
            realPlayer2 = data2.playerObject;
        }

        // 5. Final Setup: Character instantiation and Position Adjustment
        
        // Ensure Real P1 is at Spawn Point 1
        realPlayer1.transform.position = spawnPoint1.position;
        // Instantiate and position P1's character
        player1Character = InstantiateCharacter(realPlayer1, playerDataManager.player1Character);
        // Call updated function without position argument
        ApplyCharacterPositionOffset(player1Character, playerDataManager.player1Character, true); 
        
        // Ensure Real P2 is at Spawn Point 2
        realPlayer2.transform.position = spawnPoint2.position;
        // Instantiate and position P2's character
        player2Character = InstantiateCharacter(realPlayer2, playerDataManager.player2Character);
        // Call updated function without position argument
        ApplyCharacterPositionOffset(player2Character, playerDataManager.player2Character, false);
        
        // 6. Assign Health Sliders to DummyStats Components

        // Get the DummyStats component from the determined real player objects
        DummyStats stats1 = realPlayer1.GetComponent<DummyStats>();
        PlayerMovement movement1 = realPlayer1.GetComponent<PlayerMovement>();
        DummyStats stats2 = realPlayer2.GetComponent<DummyStats>();
        PlayerMovement movement2 = realPlayer2.GetComponent<PlayerMovement>();

        if (stats1 != null && stats2 != null)
        {
            // Assign the Player 1 UI Slider to the Real Player 1 object's script
            stats1.HPDislay = player1HealthSlider;
    
            // Assign the Player 2 UI Slider to the Real Player 2 object's script
            stats2.HPDislay = player2HealthSlider;
        }
        else
        {
            Debug.LogError("DummyStats component not found on one or both real player objects!");
        }
        if (movement1 != null && movement2 != null)
        {
            // Assign the Player 1 UI Slider to the Real Player 1 object's script
            movement1.staminaSlider = player1StaminaSlider;
    
            // Assign the Player 2 UI Slider to the Real Player 2 object's script
            movement2.staminaSlider = player2StaminaSlider;
        }
        else
        {
            Debug.LogError("PlayerMovement component not found on one or both real player objects!");
        }
        
        // 7. Final Debugging
        Debug.Log($"Real Player 1: {realPlayer1.name}, Character: {player1Character.name}");
        Debug.Log($"Real Player 2: {realPlayer2.name}, Character: {player2Character.name}");
    }

    // --- Helper Methods ---

    private PlayerSpawn GetPlayerSpawnData(GameObject playerObj, int desiredIndex)
    {
        PlayerInput playerInput = playerObj.GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError($"PlayerInput component not found on {playerObj.name}");
            return null;
        }

        Gamepad gamepad = playerInput.GetDevice<Gamepad>();
        if (gamepad == null)
        {
            Debug.LogWarning($"Gamepad not found for {playerObj.name}. Using a fallback ID.");
            // If no Gamepad, assign an ID that won't match the saved PlayerDataManager IDs (which are typically 1 or 2)
            return new PlayerSpawn { controllerId = -99, playerObject = playerObj, desiredPlayerIndex = desiredIndex };
        }

        return new PlayerSpawn { controllerId = gamepad.deviceId, playerObject = playerObj, desiredPlayerIndex = desiredIndex };
    }

    private GameObject InstantiateCharacter(GameObject playerContainer, int characterIndex)
    {
        int arrayIndex = characterIndex - 1;

        if (arrayIndex >= 0 && arrayIndex < characters.Length && characters[arrayIndex] != null)
        {
            // Instantiate the character prefab as a child of the player container
            GameObject character = Instantiate(characters[arrayIndex],
                playerContainer.transform.position, // Start at parent's world position
                quaternion.identity,
                playerContainer.transform);

            // ⭐ CRUCIAL FIX: Set the character's local position to zero
            // This ensures the character's root is centered exactly on the parent's pivot.
            character.transform.localPosition = Vector3.zero;

            return character;
        }
        // Add logging for clarity and return null if the character cannot be instantiated
        Debug.LogError($"Invalid character index ({characterIndex}) or character prefab not found for array index {arrayIndex}.");
        return null; // Must return a GameObject (or null) outside the 'if' block
    }
    
    private void ApplyCharacterPositionOffset(GameObject character, int characterIndex, bool isPlayer1)
    {
        if (character == null) return;
        
        Vector3 localOffset = Vector3.zero;

        switch (characterIndex)
        {
            case 1:
                break;
            case 2:
                // No offset (or zero offset)
                break;
            case 3:
                    localOffset = new Vector3(2.4f, -2.7f, 0);
                break;
            case 4:
                break;
        }
        
        // ⭐ CRUCIAL FIX: Apply the calculated offset to the local position
        character.transform.localPosition += localOffset;
    }
}