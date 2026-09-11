using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void GoToScene()
    {
        SceneManager.LoadScene("Game_Area");
    }
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Application exited.");
    }
}
