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
    private GameObject player1Character, player2Character;


    private void Awake()
    {
        playerDataManager = PlayerDataManager.Instance;   
        player1Obj = Instantiate(player1, spawnPoint1.position, spawnPoint1.rotation);
        player2Obj = Instantiate(player2, spawnPoint2.position, spawnPoint2.rotation);
        
        player1ControllerID = player1Obj.GetComponent<PlayerInput>().GetDevice<Gamepad>().deviceId;
        player2ControllerID = player2Obj.GetComponent<PlayerInput>().GetDevice<Gamepad>().deviceId;
        
        if (player1ControllerID == playerDataManager.player1Index)
        {
            realPlayer2 = player1Obj;
            //Add in the character prefab as a child to the player
            player1Character = Instantiate(characters[playerDataManager.player1Character - 1], player1Obj.transform.position, quaternion.identity,
                player1Obj.transform);
            Debug.Log("Player 1 Character is " + player1Character);

        }
        else if (player1ControllerID == playerDataManager.player2Index)
        {
            realPlayer2 = player1Obj;
            //Add in the character prefab as a child to the player
            player2Character = Instantiate(characters[playerDataManager.player2Character - 1], player2Obj.transform.position, quaternion.identity,
                player2Obj.transform);
            Debug.Log("Player 2 Character is " + player2Character);
        }
        Debug.Log(player2ControllerID + "/n" + playerDataManager.player1Index + "/n" + playerDataManager.player2Index);
        if (player2ControllerID == playerDataManager.player1Index)
        {
            realPlayer1 = player2Obj;
            //Add in the character prefab as a child to the player
            player1Character = Instantiate(characters[playerDataManager.player1Character - 1], player1Obj.transform.position, quaternion.identity,
                player1Obj.transform);
            Debug.Log("Player 1 Character is " + player1Character);
        }
        else if (player2ControllerID == playerDataManager.player2Index)
        {
            realPlayer2 = player2Obj;
            player2Character = Instantiate(characters[playerDataManager.player2Character - 1], player2Obj.transform.position, quaternion.identity,
                player2Obj.transform);
            Debug.Log("Player 2 Character is " + player2Character);
        }

        realPlayer1.transform.position = spawnPoint1.position;
        //player1Character.transform.position = realPlayer1.transform.position;
        switch (playerDataManager.player1Character)
        {
            case 1:
                player1Character.transform.position = realPlayer1.transform.position;
                break;
            case 2:
                player1Character.transform.position = new Vector3(realPlayer1.transform.position.x, realPlayer1.transform.position.y);
                break;
            case 3:
                player1Character.transform.position = new Vector3(realPlayer1.transform.position.x+2.48f, realPlayer1.transform.position.y-5.56f);
                break;
            case 4:
                player1Character.transform.position = new Vector3(realPlayer1.transform.position.x, realPlayer1.transform.position.y-0.2f);
                break;
        }
        realPlayer2.transform.position = spawnPoint2.position;
        //player2Character.transform.position = spawnPoint2.transform.position;
        switch (playerDataManager.player2Character)
        {
            case 1:
                player2Character.transform.position = realPlayer2.transform.position;
                break;
            case 2:
                player2Character.transform.position = new Vector3(realPlayer2.transform.position.x , realPlayer2.transform.position.y);
                break;
            case 3:
                player2Character.transform.position = new Vector3(realPlayer2.transform.position.x - 6.5f, realPlayer2.transform.position.y - 5.56f);
                break;
            case 4:
                player2Character.transform.position = new Vector3(realPlayer2.transform.position.x, realPlayer2.transform.position.y-0.2f);
                break;
        }
        Debug.Log(realPlayer1 + " p1");
        Debug.Log(realPlayer2 + " p2");
        Debug.Log(player1Obj + " p1");
        Debug.Log(player2Obj + " p2");
    }
}
