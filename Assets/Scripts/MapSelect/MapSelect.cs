using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MapSelect : MonoBehaviour
{
    private PlayerDataManager dataManager;
    private PlayerInput playerInput;

    private MapSelectActivation mapSelectActivation;
    private int deviceID;
    private bool isPlayer1;
    [SerializeField] private int mapIndex;

    private void Awake()
    {
        // Get the PlayerDataManager instance
        dataManager = PlayerDataManager.Instance;
        
        mapSelectActivation = MapSelectActivation.Instance;

        // This script is NOT a singleton. The PlayerInputManager creates a new instance for each player.
        playerInput = GetComponent<PlayerInput>();
        deviceID = playerInput.devices[0].deviceId;
        if (deviceID == dataManager.player1Index)
        {
            isPlayer1 = true;
        }
        else
        {
            isPlayer1 = false;
        }
    }

    public void OnButtonSouth(InputAction.CallbackContext context)
    {
        if (!isPlayer1)
        {
            SelectMap();
        }
    }
    
    public void OnDPadLeft(InputAction.CallbackContext context)
    {
        if (!isPlayer1)
        {
            if (mapIndex == 1) 
            {
                Debug.Log("Cannot select left"); 
            }
            else if (mapIndex == 2)
            {
                mapIndex = 1;
            }
            
            mapSelectActivation.ChangeMapIndicator(mapIndex);
        }
    }
    
    public void OnDPadRight(InputAction.CallbackContext context)
    {
        if (!isPlayer1)
        {
            if (mapIndex == 1)
            {
                mapIndex = 2;
            }
            else if (mapIndex == 2)
            {
                Debug.Log("Cannot select left");
            }
            
            mapSelectActivation.ChangeMapIndicator(mapIndex);
        }
    }

    private void SelectMap()
    {
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        Destroy(GameObject.FindGameObjectWithTag("Player"));
        if (mapIndex == 1)
        {
            SceneManager.LoadScene("SampleScene");
        }
        else if (mapIndex == 2)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }
}
