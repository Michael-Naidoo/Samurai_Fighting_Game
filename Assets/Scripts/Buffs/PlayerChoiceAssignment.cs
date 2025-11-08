using System;
using UnityEngine;

public class PlayerChoiceAssignment : MonoBehaviour
{
    private GameManager _gameManager => GameManager.instance;

    public bool Player1 = false;
    

    public void Choice1()
    {
        Debug.Log("Player Chose Choice 1");
        Debug.Log(_gameManager);
        if (Player1)
        {
            _gameManager.P1Choice1Selected();
        }
        else
        {
            _gameManager.P2Choice1Selected();
        }
    }
    public void Choice2()
    {
        Debug.Log("Player Chose Choice 1");
        Debug.Log(_gameManager);
        if (Player1)
        {
            _gameManager.P1Choice2Selected();
        }
        else
        {
            _gameManager.P2Choice2Selected();
        }
    }
}
