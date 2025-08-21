using System;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
    public class CharacterSelection : MonoBehaviour 
    { // The PlayerInput component attached to this object.
        private PlayerInput playerInput;

        public bool player1Connected = false;
        public int player1Index = -1;

        public int chosenCharacter = 1;

        private bool isPlayer1 = false;
        private bool selectedCharacter = false;

        // Use a reference to the PlayerDataManager
        private PlayerDataManager dataManager;

        private void Awake()
        {
            // Get the PlayerDataManager instance
            dataManager = PlayerDataManager.Instance;
        
            // This script is NOT a singleton. The PlayerInputManager creates a new instance for each player.
            playerInput = GetComponent<PlayerInput>();
        }

        public void OnButtonSouth(InputAction.CallbackContext context)
        {
            int deviceID = playerInput.devices[0].deviceId;

            if (!player1Connected)
            {
                player1Index = deviceID;
                player1Connected = true;
                Debug.Log("Player 1 connected with Device ID: " + player1Index);

                if (dataManager.player1Index == -1)
                {
                    dataManager.player1Index = player1Index;
                    isPlayer1 = true;
                }
                else if (dataManager.player2Index == -1)
                {
                    dataManager.player2Index = player1Index;
                    isPlayer1 = false;
                }
            }
            else
            {
                if (isPlayer1)
                {
                    dataManager.SelectCharacterPlayer1();
                }
                else
                {
                    dataManager.SelectCharacterPlayer2();
                }

                selectedCharacter = true;
            }
        }
        
        public void OnButtonNorth(InputAction.CallbackContext context)
        {
            if (isPlayer1)
            {
                dataManager.player1Ready = true;
            }
            else
            {
                dataManager.player2Ready = true;
            }
            
            dataManager.ReadyUp();
        }
        
        public void OnDPadUp(InputAction.CallbackContext context)
        {
            if (!selectedCharacter)
            {
                if (chosenCharacter == 1)
                {
                    Debug.Log("Cannot select up");
                }
                else if (chosenCharacter == 2)
                {
                    Debug.Log("Cannot select up");
                }
                else if (chosenCharacter == 3)
                {
                    chosenCharacter = 1;
                }
                else if (chosenCharacter == 4)
                {
                    chosenCharacter = 2;
                
                }

                SelectCharacter();
            }
        }
        
        public void OnDPadDown(InputAction.CallbackContext context)
        {
            if (!selectedCharacter)
            {
                if (chosenCharacter == 1)
                {
                    chosenCharacter = 3;
                }
                else if (chosenCharacter == 2)
                {
                    chosenCharacter = 4;
                }
                else if (chosenCharacter == 3)
                {
                    Debug.Log("Cannot select down");
                
                }
                else if (chosenCharacter == 4)
                {
                    Debug.Log("Cannot select down");
                
                }
                SelectCharacter();
            }
        }
        
        public void OnDPadLeft(InputAction.CallbackContext context)
        {
            if (!selectedCharacter)
            {
                if (chosenCharacter == 1)
                {
                    Debug.Log("Cannot select left");
                }
                else if (chosenCharacter == 2)
                {
                    chosenCharacter = 1;
                }
                else if (chosenCharacter == 3)
                {
                    Debug.Log("Cannot select left");
                
                }
                else if (chosenCharacter == 4)
                {
                    chosenCharacter = 3;
                
                }
                SelectCharacter();
            }
        }
        
        public void OnDPadRight(InputAction.CallbackContext context)
        {
            if (!selectedCharacter)
            {
                if (chosenCharacter == 1)
                {
                    chosenCharacter = 2;
                }
                else if (chosenCharacter == 2)
                {
                    Debug.Log("Cannot select right");
                }
                else if (chosenCharacter == 3)
                {
                    chosenCharacter = 4;
                }
                else if (chosenCharacter == 4)
                {
                    Debug.Log("Cannot select right");
                }
                SelectCharacter();
            }
        }

        public void SelectCharacter()
        {
            if (isPlayer1)
            {
                dataManager.player1Character = chosenCharacter;
            }
            else
            {
                dataManager.player2Character = chosenCharacter;   
            }
        }
    }
