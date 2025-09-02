using System;
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


    private void Awake()
    {
        playerDataManager = PlayerDataManager.Instance;   
        player1Obj = Instantiate(player1, spawnPoint1.position, spawnPoint1.rotation);
        player2Obj = Instantiate(player2, spawnPoint2.position, spawnPoint2.rotation);
        
        player1ControllerID = player1Obj.GetComponent<PlayerInput>().GetDevice<Gamepad>().deviceId;
        if (player1ControllerID == playerDataManager.player1Index)
        {
            realPlayer1 = player1Obj;
            player1Obj.GetComponent<WeaponsHandler>().primaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player1Primary;
            player1Obj.GetComponent<WeaponsHandler>().secondaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player1secondary;
        }
        else if (player1ControllerID == playerDataManager.player2Index)
        {
            realPlayer2 = player1Obj;
            player2Obj.GetComponent<WeaponsHandler>().primaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player2Primary;
            player2Obj.GetComponent<WeaponsHandler>().secondaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player2secondary;
        }
        
        if (player2ControllerID == playerDataManager.player1Index)
        {
            realPlayer1 = player2Obj;
            player1Obj.GetComponent<WeaponsHandler>().primaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player1Primary;
            player1Obj.GetComponent<WeaponsHandler>().secondaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player1secondary;
        }
        else if (player2ControllerID == playerDataManager.player2Index)
        {
            realPlayer2 = player2Obj;
            player2Obj.GetComponent<WeaponsHandler>().primaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player2Primary;
            player2Obj.GetComponent<WeaponsHandler>().secondaryWeapon = (WeaponsHandler.Weapons)playerDataManager.player2secondary;
        }

        realPlayer1.transform.position = spawnPoint1.position;
        realPlayer2.transform.position = spawnPoint2.position;
    }
}
