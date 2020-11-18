using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Scenes/Level1");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}