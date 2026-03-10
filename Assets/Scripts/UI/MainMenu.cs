using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    EventSystem eventSystem;
    
    public GameObject mainMenu;
    public GameObject levelSelectMenu;

    public GameObject playButton;
    public GameObject level1Button;

    void Start()
    {
        eventSystem = EventSystem.current;

        Time.timeScale = 1.0f; // Just in case
    }

    void Update()
    {
        
    }

    public void LevelSelectMenu()
    {
        mainMenu.SetActive(false);
        levelSelectMenu.SetActive(true);

        eventSystem.SetSelectedGameObject(level1Button);
    }

    public void ReturnToMainMenu()
    {
        levelSelectMenu.SetActive(false);
        mainMenu.SetActive(true);

        eventSystem.SetSelectedGameObject(playButton);
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
