using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectSceneManager : MonoBehaviour
{
    public void PvP()
    {
        SceneManager.LoadScene("Character Select");
    }
}
