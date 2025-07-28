using System;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [SerializeField]private GameObject Button1;
    [SerializeField]private GameObject Button2;
    [SerializeField]private GameObject Button3;

    private int deadPlayer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Button1 = GameObject.FindWithTag("Button1");
        Button2 = GameObject.FindWithTag("Button2");
        Button3 = GameObject.FindWithTag("Button3");
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
    }

    public void PlayerDied(int playerIndex)
    {
        Time.timeScale = 0;
        Button1.SetActive(true);
        Button2.SetActive(true);
        Button3.SetActive(true);
        GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().enabled = false;
        GameObject.FindWithTag("Player1").GetComponent<DummyStats>().HP = 100;
        GameObject.FindWithTag("Player2").GetComponent<DummyStats>().HP = 100;
        deadPlayer = playerIndex;
    }
    
    public void OnButton1()
    {
        switch (deadPlayer)
        {
            case 0:
                GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().IncreaseStrength();
                break;
            case 1:
                GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().IncreaseStrength();
                break;
        }
        Time.timeScale = 1;
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
        GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().enabled = true;
    }
    public void OnButton2()
    {
        switch (deadPlayer)
        {
            case 0:
                GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().IncreaseSpeed();
                break;
            case 1:
                GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().IncreaseSpeed();
                break;
        }
        Time.timeScale = 1;
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
        GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().enabled = true;
    }
    public void OnButton3()
    {
        switch (deadPlayer)
        {
            case 0:
                GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().IncreaseJump();
                break;
            case 1:
                GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().IncreaseJump();
                break;
        }
        Time.timeScale = 1;
        Button1.SetActive(false);
        Button2.SetActive(false);
        Button3.SetActive(false);
        GameObject.FindWithTag("Player1").GetComponent<PlayerMovement>().enabled = true;
        GameObject.FindWithTag("Player2").GetComponent<PlayerMovement>().enabled = true;
    }
}