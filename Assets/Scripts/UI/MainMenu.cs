using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    EventSystem eventSystem;
    LoadingTrigger loadingTrigger;

    public GameObject mainMenu;
    public GameObject levelSelectMenu;

    public GameObject playButton;
    public GameObject levelSelectButton;
    public GameObject level1Button;

    void Start()
    {
        eventSystem = EventSystem.current;
        loadingTrigger = GameObject.FindGameObjectWithTag("LoadingTrigger").GetComponent<LoadingTrigger>();

        Time.timeScale = 1.0f; // Just in case
    }

    void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            if (mainMenu.activeInHierarchy)
            {
                eventSystem.SetSelectedGameObject(playButton);
            }
            else
            {
                eventSystem.SetSelectedGameObject(level1Button);
            }
        }
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

        eventSystem.SetSelectedGameObject(levelSelectButton);
    }

    public void PlayLevel(string levelToLoad)
    {
        loadingTrigger.LoadScene(SceneManager.GetActiveScene().name, levelToLoad);

        // Change this to use a loading screen
        //SceneManager.LoadScene(test);
    }

    public void CreditsMenu()
    {
        SceneManager.LoadScene("Credits");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
