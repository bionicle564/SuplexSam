using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingTrigger : MonoBehaviour
{
    public void LoadScene(string currentlevel, string levelToLoad)
    {
        LoadingData.sceneToUnload = currentlevel;
        LoadingData.sceneToLoad = levelToLoad;

        SceneManager.LoadSceneAsync("Loading Screen", LoadSceneMode.Additive);
    }
}
