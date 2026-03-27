using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    EventSystem eventSystem;
    LoadingTrigger loadingTrigger;

    public GameObject menu;
    private bool isPaused = false;

    public GameObject resumeButton;
    public bool IsPaused
    {
        get { return isPaused; }
    }

    public Toggle backThrowToggle;

    void Start()
    {
        eventSystem = EventSystem.current;
        loadingTrigger = GameObject.FindGameObjectWithTag("LoadingTrigger").GetComponent<LoadingTrigger>();

        Time.timeScale = 1f; // Just in case
        menu.SetActive(false);

        if (PlayerPrefs.GetInt("BackThrowToggle") == 1)
        {
            backThrowToggle.isOn = true;
        }
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

        if (eventSystem.currentSelectedGameObject == null)
        {
            if (menu.activeInHierarchy)
            {
                eventSystem.SetSelectedGameObject(resumeButton);
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
        Time.timeScale = 1f;
        loadingTrigger.LoadScene(SceneManager.GetActiveScene().name, "Main Menu");

        // Change this to use a loading screen
        //SceneManager.LoadScene("Main Menu");
    }

    public void Toggle()
    {
        if (backThrowToggle.isOn)
        {
            PlayerPrefs.SetInt("BackThrowToggle", 1); // 0 for forwards, 1 for backwards
        }
        else
        {
            PlayerPrefs.SetInt("BackThrowToggle", 0); // 0 for forwards, 1 for backwards
        }
    }
}
