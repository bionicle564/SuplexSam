using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject menu;
    private bool isPaused = false;
    public bool IsPaused
    {
        get { return isPaused; }
    }

    void Start()
    {
        Time.timeScale = 1f; // Just in case
        menu.SetActive(false);
    }

    void Update()
    {
        if (Input.GetButtonDown(""))
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
    }

    public void UnpauseGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        menu.SetActive(false);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
