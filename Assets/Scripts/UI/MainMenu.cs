using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject levelSelectMenu;

    void Start()
    {
        Time.timeScale = 1.0f; // Just in case
    }

    void Update()
    {
        
    }

    public void LevelSelectMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);
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
