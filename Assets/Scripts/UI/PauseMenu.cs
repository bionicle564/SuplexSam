using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    EventSystem eventSystem;

    public GameObject menu;
    private bool isPaused = false;
    public bool IsPaused
    {
        get { return isPaused; }
    }

    void Start()
    {
        eventSystem = EventSystem.current;

        Time.timeScale = 1f; // Just in case
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            if (!isPaused)
            {
                PauseGame();
            }
            else 
            {
                UnpauseGame();
            }
        }
    }

    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        menu.SetActive(true);
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        menu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        // Change this to use a loading screen
        SceneManager.LoadScene("Main Menu");
    }
}
