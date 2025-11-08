using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerJoinScript: MonoBehaviour
{
    public Transform spawnPoint1, spawnPoint2;
    public GameObject player1, player2;
    private GameObject player1Obj, player2Obj;
    private int player1ControllerID, player2ControllerID;
    private GameObject realPlayer1, realPlayer2;
    private PlayerDataManager playerDataManager;
    [SerializeField] private GameObject[] characters;


    private void Awake()
    {
        playerDataManager = PlayerDataManager.Instance;   
        player1Obj = Instantiate(player1, spawnPoint1.position, spawnPoint1.rotation);
        player2Obj = Instantiate(player2, spawnPoint2.position, spawnPoint2.rotation);
        
        player1ControllerID = player1Obj.GetComponent<PlayerInput>().GetDevice<Gamepad>().deviceId;
        if (player1ControllerID == playerDataManager.player1Index)
        {
            realPlayer1 = player1Obj;
            //Add in the character prefab as a child to the player
            Instantiate(characters[playerDataManager.player1Index - 1], player1Obj.transform.position, quaternion.identity,
                player1Obj.transform);

        }
        else if (player1ControllerID == playerDataManager.player2Index)
        {
            realPlayer2 = player1Obj;
            //Add in the character prefab as a child to the player
            Instantiate(characters[playerDataManager.player2Index - 1], player1Obj.transform.position, quaternion.identity,
                player1Obj.transform);
        }
        
        if (player2ControllerID == playerDataManager.player1Index)
        {
            realPlayer1 = player2Obj;
            //Add in the character prefab as a child to the player
            Instantiate(characters[playerDataManager.player1Index - 1], player1Obj.transform.position, quaternion.identity,
                player1Obj.transform);
        }
        else if (player2ControllerID == playerDataManager.player2Index)
        {
            realPlayer2 = player2Obj;
            Instantiate(characters[playerDataManager.player1Index - 1], player1Obj.transform.position, quaternion.identity,
                player1Obj.transform);
        }

        realPlayer1.transform.position = spawnPoint1.position;
        realPlayer2.transform.position = spawnPoint2.position;
        
        Debug.Log(player1Obj);
        Debug.Log(player2Obj);
    }
}
