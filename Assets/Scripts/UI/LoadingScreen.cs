using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreenCanvas;
    public GameObject loadingScreenCanvasGroup;
    public Image loadingBarFill;

    public void LoadScene(string currentlevel, string levelToLoad)
    {
        LoadingData.sceneToUnload = currentlevel;
        LoadingData.sceneToLoad = levelToLoad;

        SceneManager.LoadSceneAsync("Loading Screen", LoadSceneMode.Additive);
    }

    /*
    public IEnumerator LoadSceneAsync(string levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);

        while (!loadOperation.isDone)
        {
            loadTimer += Time.deltaTime;
            Debug.Log($"{loadTimer}");

            float progress = Mathf.Clamp01(loadOperation.progress / 0.9f);
            loadingBarFill.fillAmount = progress;

            yield return null;
        }
    }
    */


    [Header("SlothGameGuy Code")]
    // The following code is all from YouTube channel SlothGameGuy

    //Initialize values
    AsyncOperation loadingOperation;
    AsyncOperation unLoadingOperation;
    AsyncOperation unLoadingTransitionOperation;
    //Load scene control variables
    //Have load screen show for at least 3 seconds
    float minLoadTime = 1f;
    //Timers
    float loadTimer = 0f;
    float fadeTimer = 0f;
    //Fade
    public CanvasGroup canvasGroup;
    bool fadeInLoad = true;
    bool startFadeOut = false;
    float fadeInTime = .25f;
    float fadeOutTime = .25f;
    //start unload of previous level
    bool unloadStart = true;
    bool jobsDone = false;
    //Porgress
    float progressValue = 0f;

    void Update()
    {
        if (loadingOperation != null)
        {
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            loadingBarFill.fillAmount = progress;
        }

        //Fade-in
        if (fadeInLoad)
        {
            //Fade in operation adjusts alpha for the canvas group containing all of your loading scene images
            if (loadTimer < fadeInTime)
            {
                canvasGroup.alpha = Mathf.Lerp(0, 1, loadTimer / fadeInTime);
            }
            else
            {
                //Once fade in complete, set alpha purposefully
                canvasGroup.alpha = 1;
                //Unload previous scene
                if (unloadStart)
                {
                    unLoadingOperation = SceneManager.UnloadSceneAsync(LoadingData.sceneToUnload, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                    unloadStart = false;
                }

                //Load next scene
                if (unLoadingOperation.isDone)
                {
                    loadingOperation = SceneManager.LoadSceneAsync(LoadingData.sceneToLoad, LoadSceneMode.Additive);
                    //For preventing load screen from flashing too quickly
                    loadingOperation.allowSceneActivation = false;
                    fadeInLoad = false;
                }
            }
        }
        else
        //Progress meter and duration control
        {
            //Load percent text output
            progressValue = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            //For preventing load screen from flashing too quickly even if loading is done
            if ((loadTimer > minLoadTime) && (Mathf.Approximately(loadingOperation.progress, .9f)))
            {
                loadingOperation.allowSceneActivation = true;
            }

            //If level is loaded, start the fade out process
            if (!startFadeOut && loadingOperation.isDone)
            {
                startFadeOut = true;
                //Set active scene to your newly loaded scene to prevent crossover code issues
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(LoadingData.sceneToLoad));
                //Set fade-out timer to 0 
                fadeTimer = 0f;
            }

            //Fade out operation adjusts alpha for the canvas group containing all of your loading scene images
            if (startFadeOut && (fadeTimer < fadeOutTime))
            {
                canvasGroup.alpha = Mathf.Lerp(1, 0, fadeTimer / fadeOutTime);
            }
            else if (startFadeOut && !jobsDone && (fadeTimer >= fadeOutTime))
            {
                //Once fade out complete, set alpha purposefully
                canvasGroup.alpha = 0;
                //unload loading menu scene
                unLoadingTransitionOperation = SceneManager.UnloadSceneAsync("Loading Screen", UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
                jobsDone = true;
            }
        }

        //Increment total load timer
        loadTimer += Time.deltaTime;

        //Increment total load timer
        fadeTimer += Time.deltaTime;

    }
}
