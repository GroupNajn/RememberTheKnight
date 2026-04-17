using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    Dictionary<string, AsyncOperation> pendingLoads = new Dictionary<string, AsyncOperation>();

    Animator transitionAnimator;
    bool useTransition;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnActiveSceneChanged;
        transitionAnimator = GameObject.FindGameObjectWithTag("BlackFade").GetComponent<Animator>();
    }

    private void Start()
    {
        transitionAnimator.ResetTrigger("FadeFromBlack");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            Event_System.instance.OnLoadScenes.Invoke();
        } 
    }

    public void LoadScene(string sceneName)
    {
        if (pendingLoads.ContainsKey(sceneName))
        {
            return;
        }

        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            // Scene is already loaded, do nothing
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false;
         
        while (asyncLoad.progress < 0.9f)
        {
            yield return null;
        }

        pendingLoads[sceneName] = asyncLoad;
    }

    public void ActivateSceneNoTransition(string sceneName)
    {
        Debug.Log("Activating scene without transition");
        useTransition = false;
        //SceneManager.LoadScene(sceneName);
        ActivateScene(sceneName);
        //Time.timeScale = 1f;
    }

    public void ActivateSceneTransition(string sceneName)
    {
        useTransition = true;
        //ActivateSceneTransition(sceneName);
        ActivateScene(sceneName);
    }

    void ActivateScene(string sceneName)
    {
        Debug.Log("Activating scene " + sceneName);
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            if (useTransition)
            {
                transitionAnimator.SetTrigger("FadeToBlack");
            }

            SceneManager.SetActiveScene(scene);
            return;
        }

        Debug.Log($"{sceneName} was not loaded, checking if preloaded");

        if (pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            if (useTransition)
            {
                transitionAnimator.SetTrigger("FadeToBlack");
            }

            StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            return;
        }

        Debug.Log($"{sceneName} was not preloaded, loading it directly");

        if (useTransition)
        {
            transitionAnimator.SetTrigger("FadeToBlack");
        }

        SceneManager.LoadScene(sceneName);
    }

    //IEnumerator LoadSceneWithTransition(string sceneName)
    //{
    //    transitionAnimator.SetTrigger("FadeToBlack");
    //    yield return null; // Wait a frame to let the animator switch state
    //    yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    //    SceneManager.LoadScene(sceneName);
    //    Time.timeScale = 1f;
    //}

    IEnumerator ActivatePendingLoad(string sceneName, AsyncOperation asyncOperation)
    {
        if (asyncOperation == null)
        {
            yield break;
        }

        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            yield return null;
        }

        pendingLoads.Remove(sceneName);

        Scene scene = SceneManager.GetSceneByName(sceneName);

        while (!scene.isLoaded)
        {
            yield return null;
        }

        if (scene.IsValid())
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene loaded " + scene.name);
        if (transitionAnimator)
        {
            if (useTransition)
            {
                StartCoroutine(FadeOut());
            }
        }
        else
        {
            Debug.Log("transitionAnimator is null, if this is not when starting the game, this is a problem");
        }
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log($"Activated scene {newScene} from {previousScene}");
        if (transitionAnimator != null)
        {
            if (useTransition)
            {
                StartCoroutine(FadeOut());
            }
        }
        else
        {
            Debug.Log("transitionAnimator is null, if this is not when starting the game, this is a problem");
        }

        //if (previousScene.IsValid())
        //{
        //    StartCoroutine(UnloadSceneAsync(previousScene)); 
        //}

        int count = SceneManager.sceneCount;
        List<Scene> scenes = new List<Scene>();

        for (int i = 0; i < count; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }

        foreach (Scene scene in scenes)
        {
            if (scene != newScene && scene.IsValid() && scene.isLoaded)
            {
                StartCoroutine(UnloadSceneAsync(scene)); 
            }
        }
    }

    IEnumerator UnloadSceneAsync(Scene scene)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);

        Debug.Log($"Trying to unload {scene.name}");

        if (asyncUnload == null)
        {
            Debug.Log("UnloadSceneAsync operation is null");
            yield break;
        }

        Debug.Log("UnloadSceneAsync operation is not null"); 

        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        Debug.Log("asyncUnload is done");
    }

    IEnumerator FadeOut()
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        // Add event to trigger when the fade-out animation is complete, if needed
    }
}