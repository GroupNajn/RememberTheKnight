using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    public static GlobalSceneManager Instance { get; private set; }

    Dictionary<string, AsyncOperation> pendingLoads = new Dictionary<string, AsyncOperation>();
    HashSet<string> loadingScenes = new HashSet<string>();

    [SerializeField] private Animator transitionAnimator;
    bool useTransition;
    public bool isTransitioning { get; private set; } = false;

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

        yield return StartCoroutine(FadeToBlack());

        Scene targetScene = SceneManager.GetSceneByName(sceneName);

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

        Event_System.instance.OnLoadScenes.Invoke();

        if (sceneName == SceneData.Instance[2]) // Lobby loaded
        {
            Event_System.instance.OnLobbyLoaded?.Invoke();
        }

        try // setting parameter for FMOD
        {
            RuntimeManager.StudioSystem.setParameterByNameWithLabel("Scene", sceneName);
        }
        catch
        {
            Debug.Log($"FATAL ERROR PREVENTED: No Scene in FMOD called {sceneName}");
        }

        yield return StartCoroutine(FadeFromBlack());

        isTransitioning = false;

        Event_System.instance.OnSceneTransitionDone?.Invoke();

       
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
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null;
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    }

    IEnumerator FadeFromBlack()
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return null;
        Time.timeScale = 1.0f;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    }
}