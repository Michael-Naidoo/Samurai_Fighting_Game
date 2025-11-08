using System;
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
    public bool player1chosen = false;
    public bool player2chosen = false;
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