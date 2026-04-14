using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class CreditsMenu : MonoBehaviour
{
    EventSystem eventSystem;

    public GameObject button;

    void Start()
    {
        eventSystem = EventSystem.current;
    }

    void Update()
    {
        eventSystem.SetSelectedGameObject(button);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
