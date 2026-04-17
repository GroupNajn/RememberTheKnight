using System.Collections;
using System.Collections.Generic;
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

    public void LoadScene(string sceneName)
    {
        if (pendingLoads.ContainsKey(sceneName) || loadingScenes.Contains(sceneName))
        {
            Debug.Log($"{sceneName} is already loading");
            return;
        }

        loadingScenes.Add(sceneName);
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            // Scene is already loaded
            loadingScenes.Remove(sceneName);
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
        ActivateScene(sceneName);
    }

    public void ActivateSceneTransition(string sceneName)
    {
        useTransition = true;
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
                StartCoroutine(TransitionToScene(sceneName, null, true));
                return;
            }

            SceneManager.SetActiveScene(scene);
            return;
        }

        Debug.Log($"{sceneName} was not loaded, checking if preloaded");

        if (pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            Debug.Log($"{sceneName} is preloaded, use transition is {useTransition}");
            if (useTransition)
            {
                Debug.Log($"Starting transition to {sceneName}");
                StartCoroutine(TransitionToScene(sceneName, asyncLoad, true));
                return;
            }

            StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            return;
        }

        if (loadingScenes.Contains(sceneName))
        {
            Debug.Log($"{sceneName} is currently loading, waiting to activate");
            StartCoroutine(WaitForSceneLoad(sceneName));
            return;
        }

        Debug.Log($"{sceneName} was not preloaded, loading it directly");

        if (useTransition)
        {
            StartCoroutine(TransitionToScene(sceneName, asyncLoad, false));
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator WaitForSceneLoad(string sceneName)
    {
        bool shouldTransition = useTransition;

        while (!pendingLoads.ContainsKey(sceneName))
        {
            yield return null;
        }

        if (!pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            if (shouldTransition)
            {
                StartCoroutine(TransitionToScene(sceneName, asyncLoad, true));
            }
            else
            {
                StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            }
        }
    }

    IEnumerator TransitionToScene(string sceneName, AsyncOperation pendingAsyncOp, bool isPreloaded)
    {
        if (isTransitioning)
        {
            Debug.Log("Already transitioning, queueing scene activation");
            yield break;
        }

        Debug.Log($"Tranisitioning to {sceneName}");

        isTransitioning = true;

        Debug.Log("Starting fade to black");
        yield return StartCoroutine(FadeToBlack());

        Debug.Log("Returned after FadeToBlack");

        Scene targetScene = SceneManager.GetSceneByName(sceneName);

        Debug.Log($"targetScene is: {targetScene.name}");

        if (pendingAsyncOp != null)
        {
            Debug.Log($"Activating preloaded scene {sceneName}");
            pendingAsyncOp.allowSceneActivation = true;
            while (!pendingAsyncOp.isDone)
            {
                Debug.Log($"{sceneName} is loading");
                yield return null;
            }
            Debug.Log($"{sceneName} is done loading");
            pendingLoads.Remove(sceneName);

            while (!targetScene.isLoaded)
            {
                yield return null;
            }

            SceneManager.SetActiveScene(targetScene);
        }
        else
        {
            if (!targetScene.isLoaded)
            {
                Debug.Log($"Loading scene {sceneName} for the first time");
                LoadSceneMode loadMode = SceneManager.sceneCount > 1 ? LoadSceneMode.Single : LoadSceneMode.Additive;

                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, loadMode);
                asyncLoad.allowSceneActivation = true;
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                targetScene = SceneManager.GetSceneByName(sceneName);
                while (!targetScene.isLoaded)
                {
                    yield return null;
                }

                if (loadMode == LoadSceneMode.Additive)
                {
                    SceneManager.SetActiveScene(targetScene);
                }
            }
            else
            {
                SceneManager.SetActiveScene(targetScene);
            }
        }

        //SceneManager.SetActiveScene(targetScene);

        // 3) Unload old scene
        if (SceneManager.sceneCount > 1)
        {
            yield return StartCoroutine(UnloadOtherScenes(targetScene));
        }

        // 4) Invoke event system
        Event_System.instance.OnLoadScenes.Invoke();

        // 5) Fade from black
        yield return StartCoroutine(FadeFromBlack());

        isTransitioning = false;
    }

    IEnumerator UnloadOtherScenes(Scene activeScene)
    {

        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        Debug.Log("Reading pendingLoads");
        foreach (var item in pendingLoads)
        {
            asyncOperations.Add(item.Value);
        }

        Debug.Log("Looping through asyncOperations");
        foreach (var operation in asyncOperations)
        {
            operation.allowSceneActivation = true;

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        pendingLoads.Clear();

        yield return null;

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

            if (SceneManager.sceneCount > 1)
            {
                yield return StartCoroutine(UnloadOtherScenes(scene));
            }
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

        if (transitionAnimator != null && isTransitioning)
        {
        }

        //if (transitionAnimator != null)
        //{
        //    if (!isTransitioning && useTransition)
        //    {
        //        StartCoroutine(FadeFromBlack());
        //    }
        //}
        //else
        //{
        //    Debug.Log("transitionAnimator is null, if this is not when starting the game, this is a problem");
        //}

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
        Debug.Log("Fading to black");
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null;
        Debug.Log($"Waiting for {transitionAnimator.GetCurrentAnimatorStateInfo(0).length} seconds");
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        Debug.Log("Faded to black");
    }

    IEnumerator FadeFromBlack()
    {
        Debug.Log("Fading from black");
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return null;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        // Add event to trigger when the fade-out animation is complete, if needed
    }
}