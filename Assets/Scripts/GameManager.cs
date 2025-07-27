using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public Transform[] spawnPoints;
    private int currentPlayerCount = 0;
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerInput.transform.position = spawnPoints[currentPlayerCount].position;
        playerInput.transform.rotation = spawnPoints[currentPlayerCount].rotation;
        currentPlayerCount++;
    }
}