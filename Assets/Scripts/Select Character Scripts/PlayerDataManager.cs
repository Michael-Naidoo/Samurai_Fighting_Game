using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { get; private set; }
    
    public int player1Index = -1;

    public int player1Character = 1;
    
    public int player2Index = -1;

    public int player2Character = 1;

    public bool player1Ready = false;
    public bool player2Ready = false;

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
    }

    public void SelectCharacterPlayer1()
    {
        
    }
    public void SelectCharacterPlayer2()
    {
        
    }

    public void ReadyUp()
    {
        if (player1Ready && player2Ready)
        {
            SceneManager.LoadScene("Map Select");
        }
    }
}