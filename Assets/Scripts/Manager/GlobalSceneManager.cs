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

        if (pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            Debug.Log("Activating pending load for scene: " + sceneName);
            if (useTransition)
            {
                StartCoroutine(TransitionToScene(sceneName, asyncLoad, true));
                return;
            }

            StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            return;
        }

        if (loadingScenes.Contains(sceneName))
        {
            StartCoroutine(WaitForSceneLoad(sceneName));
            return;
        }

        if (useTransition)
        {
            StartCoroutine(TransitionToScene(sceneName, asyncLoad, false));
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
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
            yield break;
        }

        isTransitioning = true;

        Debug.Log($"Transitioning to scene: {sceneName} with transition: {useTransition} and preloaded: {isPreloaded}");
        yield return StartCoroutine(FadeToBlack());
        Debug.Log("Fade to black completed");

        Scene targetScene = SceneManager.GetSceneByName(sceneName);
        Debug.Log($"Target scene: {targetScene.name}, loaded: {targetScene.isLoaded}, valid: {targetScene.IsValid()}");

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

            SceneManager.SetActiveScene(targetScene);
        }
        else
        {
            if (!targetScene.isLoaded)
            {
                LoadSceneMode loadMode = SceneManager.sceneCount > 1 ? LoadSceneMode.Single : LoadSceneMode.Additive;

                if (isPreloaded)
                {
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

                    SceneManager.SetActiveScene(targetScene);
                }
                else
                {
                    SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                }
            }
            else
            {
                SceneManager.SetActiveScene(targetScene);
            }
        }

        if (SceneManager.sceneCount > 1)
        {
            yield return StartCoroutine(UnloadOtherScenes(targetScene));
        }

        Debug.Log("Scene transition complete, invoking OnLoadScenes event");
        Event_System.instance.OnLoadScenes.Invoke();

        yield return StartCoroutine(FadeFromBlack());

        isTransitioning = false;
    }

    IEnumerator UnloadOtherScenes(Scene activeScene)
    {

        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        foreach (var item in pendingLoads)
        {
            asyncOperations.Add(item.Value);
        }

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

    IEnumerator ActivatePendingLoad(string sceneName, AsyncOperation asyncOperation)
    {
        if (asyncOperation == null)
        {
            Debug.LogWarning("AsyncOperation for scene " + sceneName + " is null. Cannot activate scene.");
            yield break;
        }

        Debug.Log("Activating pending load in coroutine for scene: " + sceneName);
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

    IEnumerator UnloadSceneAsync(Scene scene)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);

        if (asyncUnload == null)
        {
            yield break;
        }

        while (!asyncUnload.isDone)
        {
            yield return null;
        }
    }

    IEnumerator FadeToBlack()
    {
        Debug.Log("Fading to black");
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null;
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    }

    IEnumerator FadeFromBlack()
    {
        Debug.Log("Fading from black");
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return null;
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
        // Add event to trigger when the fade-out animation is complete if needed
    }
}