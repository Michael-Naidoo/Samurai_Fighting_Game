using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PostFightMenu : MonoBehaviour
{
    private int selectedScene = 0;

    [SerializeField] private GameObject selectedButton0;
    [SerializeField] private GameObject selectedButton1;
    [SerializeField] private GameObject selectedButton2;
    [SerializeField] private GameObject selectedButton3;
    
    public void OnButtonSouth(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            switch (selectedScene)
            {
                case 0:
                    SceneManager.LoadScene("SampleScene");
                    break;
                case 1:
                    SceneManager.LoadScene("Character Select");
                    break;
                case 2:
                    SceneManager.LoadScene("Map Select");
                    break;
                case 3:
                    SceneManager.LoadScene("Main Menu");
                    break;
                default:
                    return;
            }
        }
    }

    public void onDPadUp(InputAction.CallbackContext context)
    {
        if (context.started && selectedScene > 0)
        {
            selectedScene--;
            switch (selectedScene)
            {
                case 0:
                    selectedButton0.SetActive(true);
                    selectedButton1.SetActive(false);
                    selectedButton2.SetActive(false);
                    selectedButton3.SetActive(false);
                    break;
                case 1:
                    selectedButton0.SetActive(false);
                    selectedButton1.SetActive(true);
                    selectedButton2.SetActive(false);
                    selectedButton3.SetActive(false);
                    break;
                case 2:
                    selectedButton0.SetActive(false);
                    selectedButton1.SetActive(false);
                    selectedButton2.SetActive(true);
                    selectedButton3.SetActive(false);
                    break;
                case 3:
                    selectedButton0.SetActive(false);
                    selectedButton1.SetActive(false);
                    selectedButton2.SetActive(false);
                    selectedButton3.SetActive(true);
                    break;
                default:
                    return;
            }
        }
    }
    public void onDPadDown(InputAction.CallbackContext context)
    {
        if (context.started && selectedScene < 3)
        {
            selectedScene++;
            switch (selectedScene)
            {
                case 0:
                    selectedButton0.SetActive(true);
                    selectedButton1.SetActive(false);
                    selectedButton2.SetActive(false);
                    selectedButton3.SetActive(false);
                    break;
                case 1:
                    selectedButton0.SetActive(false);
                    selectedButton1.SetActive(true);
                    selectedButton2.SetActive(false);
                    selectedButton3.SetActive(false);
                    break;
                case 2:
                    selectedButton0.SetActive(false);
                    selectedButton1.SetActive(false);
                    selectedButton2.SetActive(true);
                    selectedButton3.SetActive(false);
                    break;
                case 3:
                    selectedButton0.SetActive(false);
                    selectedButton1.SetActive(false);
                    selectedButton2.SetActive(false);
                    selectedButton3.SetActive(true);
                    break;
                default:
                    return;
            }
        }
    }
}
