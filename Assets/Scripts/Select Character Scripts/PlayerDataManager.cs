using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    
    public int player1Index = -1;

    public int player1Character = 1;
    public int player1Primary;
    public int player1secondary;
    
    public int player2Index = -1;

    public int player2Character = 1;
    public int player2Primary;
    public int player2secondary;
    
    public bool player1Ready = false;
    public bool player2Ready = false;
    [SerializeField] private GameObject characterSelectPrefab;
    [SerializeField]private GameObject mapSelectPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (SceneManager.GetActiveScene().name == "Character Select")
        {
            gameObject.GetComponent<PlayerInputManager>().playerPrefab = characterSelectPrefab;
        }
        else if (SceneManager.GetActiveScene().name == "Map Select")
        {
            gameObject.GetComponent<PlayerInputManager>().playerPrefab = mapSelectPrefab;
        }
        else
        {
            gameObject.GetComponent<PlayerInputManager>().enabled = false;
        }
        
    }

    public void SelectCharacterPlayer1()
    {
        switch (player1Character)
        {
            case 1:
                player1Primary = 1;
                player1secondary = 2;
                break;
            case 2 :
                player1Primary = 1;
                player1secondary = 3;
                break;
            case 3:
                player1Primary = 1;
                player1secondary = 4;
                break;
            case 4 :
                player1Primary = 2;
                player1secondary = 4;
                break;
        }
    }
    public void SelectCharacterPlayer2()
    {
        switch (player2Character)
        {
            case 1:
                player2Primary = 1;
                player2secondary = 2;
                break;
            case 2 :
                player2Primary = 1;
                player2secondary = 3;
                break;
            case 3:
                player2Primary = 1;
                player2secondary = 4;
                break;
            case 4 :
                player2Primary = 2;
                player2secondary = 4;
                break;
        }
    }

    public void ReadyUp()
    {
        if (player1Ready && player2Ready)
        {
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            Destroy(GameObject.FindGameObjectWithTag("Player"));
            SceneManager.LoadScene("Map Select");
        }
    }
}