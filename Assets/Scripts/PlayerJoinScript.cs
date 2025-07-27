using System;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJoinScript: MonoBehaviour
{
    public Transform spawnPoint1, spawnPoint2;
    public GameObject player1, player2;
    

    private void Awake()
    {
        Instantiate(player1, spawnPoint1.position, spawnPoint1.rotation);
        Instantiate(player2, spawnPoint2.position, spawnPoint2.rotation);
        
        
    }
}
