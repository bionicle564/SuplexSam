using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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

    void Start()
    {
        eventSystem = EventSystem.current;
        loadingTrigger = GameObject.FindGameObjectWithTag("LoadingTrigger").GetComponent<LoadingTrigger>();

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
}
