using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour
{
    public static ResetScript Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnResetButton1(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }
    public void OnResetButton2(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("Character Select");
    }
    public void OnResetButton3(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene("SampleScene");
    }
}
