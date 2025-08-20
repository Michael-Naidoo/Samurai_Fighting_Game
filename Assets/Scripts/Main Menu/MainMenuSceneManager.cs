using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuSceneManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Mode Select Menu");
    }
    public void Controls()
    {
        SceneManager.LoadScene("Controls");
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
