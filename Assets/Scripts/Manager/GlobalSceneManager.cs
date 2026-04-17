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
    HashSet<string> loadingScenes = new HashSet<string>();

    Animator transitionAnimator;
    bool useTransition;
    bool isTransitioning;

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

        loadingScenes.Add(sceneName);
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

        loadingScenes.Remove(sceneName);
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
                StartCoroutine(TransitionToScene(sceneName, null));
            }

            SceneManager.SetActiveScene(scene);
            return;
        }

        Debug.Log($"{sceneName} was not loaded, checking if preloaded");

        if (pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            if (useTransition)
            {
                StartCoroutine(TransitionToScene(sceneName, asyncLoad));
            }

            StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            return;
        }

        Debug.Log($"{sceneName} was not preloaded, loading it directly");

        if (useTransition)
        {
            StartCoroutine(TransitionToScene(sceneName, asyncLoad));
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator TransitionToScene(string sceneName, AsyncOperation pendingAsyncOp)
    {
        if (isTransitioning)
        {
            yield break;
        }

        isTransitioning = true;

        // 1) Fade to black
        yield return StartCoroutine(FadeToBlack());

        Scene targetScene = SceneManager.GetSceneByName(sceneName);

        // 2) Activate or load scene
        if (pendingAsyncOp != null)
        {
            pendingAsyncOp.allowSceneActivation = true;
            while (!pendingAsyncOp.isDone)
            {
                yield return null;
            }
            pendingLoads.Remove(sceneName);

            while (!targetScene.isLoaded)
            {
                yield return null;
            }
        }

        SceneManager.SetActiveScene(targetScene);

        // 3) Unload old scene
        yield return StartCoroutine(UnloadOtherScenes(targetScene));

        // 4) Invoke event system
        Event_System.instance.OnLoadScenes.Invoke();

        // 5) Fade from black
        yield return StartCoroutine(FadeFromBlack());

        isTransitioning = false;
    }

    IEnumerator UnloadOtherScenes(Scene activeScene)
    {
        int count = SceneManager.sceneCount;
        List<Scene> scenes = new List<Scene>();

        for (int i = 0; i < count; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }

        foreach (Scene scene in scenes)
        {
            if (scene != activeScene && scene.IsValid() && scene.isLoaded)
            {
                yield return StartCoroutine(UnloadSceneAsync(scene));
            }
        }
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

        if (useTransition)
        {
            StartCoroutine(FadeToBlack());
            yield return null;
            yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
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
        //Debug.Log("Scene loaded " + scene.name);
        //if (transitionAnimator)
        //{
        //    if (useTransition)
        //    {
        //        StartCoroutine(FadeFromBlack());
        //    }
        //}
        //else
        //{
        //    Debug.Log("transitionAnimator is null, if this is not when starting the game, this is a problem");
        //}
    }

    private void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        Debug.Log($"Activated scene {newScene} from {previousScene}");
        if (transitionAnimator != null)
        {
            if (!isTransitioning && useTransition)
            {
                StartCoroutine(FadeFromBlack());
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

        //int count = SceneManager.sceneCount;
        //List<Scene> scenes = new List<Scene>();
        
        //for (int i = 0; i < count; i++)
        //{
        //    scenes.Add(SceneManager.GetSceneAt(i));
        //}

        //foreach (Scene scene in scenes)
        //{
        //    if (scene != newScene && scene.IsValid() && scene.isLoaded)
        //    {
        //        StartCoroutine(UnloadSceneAsync(scene)); 
        //    }
        //}
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

    IEnumerator FadeToBlack()
    {
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null;
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        Event_System.instance.OnLoadScenes.Invoke();
        Debug.Log("Faded to black");
    }

    IEnumerator FadeFromBlack()
    {
        Debug.Log("Fading from black");
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return null;
        yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        // Add event to trigger when the fade-out animation is complete, if needed
    }
}