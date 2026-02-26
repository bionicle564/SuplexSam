using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1.0f; // Just in case
    }

    void Update()
    {
        
    }

    public void PlayLevel(string test)
    {
        // Change this to use a loading screen
        SceneManager.LoadScene(test);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
