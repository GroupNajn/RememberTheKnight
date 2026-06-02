using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlobalSceneManager : MonoBehaviour
{
    /// <summary>
    /// Made by Lukas 2026-04-10
    /// 
    /// Manages scene loading and transitions for the game
    /// Asynchronous scene loading with support for pre-loading scenes in the background to reduce load times during scene switches
    /// The starting of the asynchronous loading is mainly triggered when the screen is black 
    ///     while showing the king card of the currently selected family but not neccisary
    ///     
    /// Updated by Lukas over the next 2-3 weeks
    /// </summary>

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

    // Preloads a scene asynchronous in the background to lower load times when switching scenes
    IEnumerator LoadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            // Scene is already loaded
            loadingScenes.Remove(sceneName);
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = false; // Ensures that the scene isn't activated before it should

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

    // Activates a scene and unloads the all the other scenes afterwards
    void ActivateScene(string sceneName)
    {
        Scene scene = SceneManager.GetSceneByName(sceneName);
        if (scene.isLoaded)
        {
            // The scenes is already loaded and activated and nothing should happen, should never enter if everything is correctly done
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
            // Scene is preloaded and only needs to be activated
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
            // Scene is being preloaded
            StartCoroutine(WaitForSceneLoad(sceneName));
            return;
        }

        // Scene is not preloaded or being preloaded and needs to be loaded and activated directly, with or without trans
        if (useTransition)
        {
            StartCoroutine(TransitionToScene(sceneName, asyncLoad, false));
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    IEnumerator WaitForSceneLoad(string sceneName)
    {
        // Save transition state in case it changes while waiting
        bool shouldTransition = useTransition;

        while (!pendingLoads.ContainsKey(sceneName))
        {
            // Wait until scene is done preloading
            yield return null;
        }

        if (pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            // Scene is done preloading and only needs to be activated
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
            // Already transitioning, do nothing
            yield break;
        }

        isTransitioning = true;
        WorldSoundFXManager.instance.musicInstance.stop(STOP_MODE.ALLOWFADEOUT);

        yield return StartCoroutine(FadeToBlack());

        Scene targetScene = SceneManager.GetSceneByName(sceneName);

        if (pendingAsyncOp != null)
        {
            // Scene is preloaded and only needs to be activated
            yield return StartCoroutine(ActivatePendingLoad(sceneName, pendingAsyncOp));
        }
        else if (!targetScene.isLoaded)
        {
            // Scene is not loaded yet
            if (isPreloaded)
            {
                // Scene is preloaded but no async operation is saved for it
                AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                asyncLoad.allowSceneActivation = true;
                yield return StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            }
            else
            {
                // Scene is not preloaded and needs to be loaded and activated by itself
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
            }
        }
        else
        {
            // Scene is already loaded and only needs to be activated
            SceneManager.SetActiveScene(targetScene);
        }

        targetScene = SceneManager.GetSceneByName(sceneName); // Reset the struct reference to the scene

        if (SceneManager.sceneCount > 1)
        {
            // Unload all the other scenes if there are any
            yield return StartCoroutine(UnloadOtherScenes(targetScene));
        }

        yield return null;

        // Scene is fully loaded, screen is still black and it's safe to preload scenes now
        Event_System.instance.OnLoadScenes.Invoke();

        if (sceneName == SceneData.Instance[2]) // Lobby loaded
        {
            Event_System.instance.OnLobbyLoaded?.Invoke();
        }

        try // setting parameter for FMOD
        {
            RuntimeManager.StudioSystem.setParameterByNameWithLabel("Scene", sceneName);
            WorldSoundFXManager.instance.musicInstance.start();

        }
        catch
        {
            Debug.Log($"FATAL ERROR PREVENTED: No Scene in FMOD called {sceneName}");
        }

        yield return null;
        // Start fading out from black
        yield return StartCoroutine(FadeFromBlack());

        isTransitioning = false;

        Event_System.instance.OnSceneTransitionDone?.Invoke();
    }

    // Unload scenes that aren't supposed to be loaded
    IEnumerator UnloadOtherScenes(Scene activeScene)
    {
        List<AsyncOperation> asyncOperations = new List<AsyncOperation>();
        foreach (var item in pendingLoads)
        {
            asyncOperations.Add(item.Value);
        }

        foreach (var operation in asyncOperations)
        {
            // Activate and unload all scens without setting them as main active scene
            operation.allowSceneActivation = true;

            while (!operation.isDone)
            {
                yield return null;
            }
        }

        // Clear pending loads as all scenes have been loaded
        pendingLoads.Clear();

        yield return null; // Wait until the next frame

        int count = SceneManager.sceneCount;
        List<Scene> scenes = new List<Scene>();

        for (int i = 0; i < count; i++)
        {
            // Save a reference to all the active scenes
            scenes.Add(SceneManager.GetSceneAt(i));
        }

        foreach (Scene scene in scenes)
        {
            if (scene != activeScene && scene.IsValid() && scene.isLoaded)
            {
                // Unload every scene that isn't the desired scene, can be unloaded
                yield return StartCoroutine(UnloadSceneAsync(scene));
            }
        }
    }

    // Activate a scens that is waiting to be loaded
    IEnumerator ActivatePendingLoad(string sceneName, AsyncOperation asyncOperation)
    {
        if (asyncOperation == null)
        {
            // Just a fail safe in case the async operation is null
            Debug.LogWarning("AsyncOperation for scene " + sceneName + " is null. Cannot activate scene.");
            yield break;
        }

        // Allow the scene to be activated
        asyncOperation.allowSceneActivation = true;

        while (!asyncOperation.isDone)
        {
            // Wait until the async operation is done
            yield return null;
        }

        // Remove the scene from pending loads as it's done loading
        pendingLoads.Remove(sceneName);

        Scene scene = SceneManager.GetSceneByName(sceneName);

        while (!scene.isLoaded)
        {
            // Wait until the scene is fully loaded
            yield return null;
        }

        if (scene.IsValid())
        {
            // Activate the scene if it's valid
            SceneManager.SetActiveScene(scene);
        }
    }

    // Unloads scene asynchronous
    IEnumerator UnloadSceneAsync(Scene scene)
    {
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(scene);

        if (asyncUnload == null)
        {
            yield break;
        }

        while (!asyncUnload.isDone)
        {
            // Wait unitl the scene is fully unloaded
            yield return null;
        }
    }

    IEnumerator FadeToBlack()
    {
        transitionAnimator.SetTrigger("FadeToBlack");
        yield return null;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    }

    IEnumerator FadeFromBlack()
    {
        transitionAnimator.SetTrigger("FadeFromBlack");
        yield return null;
        yield return new WaitForSecondsRealtime(transitionAnimator.GetCurrentAnimatorStateInfo(0).length); // Wait for the fade-out animation to complete
    }
}