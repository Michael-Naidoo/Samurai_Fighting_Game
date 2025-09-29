using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class ReturnTo : MonoBehaviour
{
    public void OnButtonSouth(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
