using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTrigger : MonoBehaviour
{
    LoadingTrigger loadingTrigger;
    public string levelToLoad;

    void Start()
    {
        loadingTrigger = GameObject.FindGameObjectWithTag("LoadingTrigger").GetComponent<LoadingTrigger>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            loadingTrigger.LoadScene(SceneManager.GetActiveScene().name, levelToLoad);
        }
    }
}
