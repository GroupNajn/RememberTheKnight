using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ThisSceneManager : MonoBehaviour
{
    public static ThisSceneManager Instance { get; private set; }

    [field: SerializeField] public Transform PlayerSpawnPosition { get; private set; }

    readonly Dictionary<string, AsyncOperation> pendingLoads = new Dictionary<string, AsyncOperation>();

    PlayerInput playerInput;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }
    }

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }


    public void LoadSceneNow(string sceneName)
    {
        // Instantly loads the new scene and unloads the other scenes
        playerInput.enabled = true;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadScene(string sceneName)
    {
        // Loads the new scene in the background
        StartCoroutine(LoadSeneAsync(sceneName));
    }

    public void ActivateScene(string sceneName)
    {
        // Activates the scene if it's already loaded, otherwise it loads the scene
        Scene scene = SceneManager.GetSceneByName(sceneName);

        if (scene.isLoaded && scene == SceneManager.GetSceneAt(SceneManager.sceneCount - 1))
        {
            SceneManager.SetActiveScene(scene);
            return;
        }

        if (pendingLoads.TryGetValue(sceneName, out AsyncOperation asyncLoad))
        {
            StartCoroutine(ActivatePendingLoad(sceneName, asyncLoad));
            return;
        }

        // If the scene is not loaded and not pending, load it immediately
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSeneAsync(string sceneName)
    {
        // Load the scene asynchronously in the background, but do not activate it until ActivateScene is called

        if (pendingLoads.ContainsKey(sceneName) || SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            // Scene is already loaded or being loaded, do nothing
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

    IEnumerator ActivatePendingLoad(string sceneName, AsyncOperation asyncOperation)
    {
        // Activate a scene that is currently being loaded in the background

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

        if (scene.IsValid() && scene.isLoaded)
        {
            SceneManager.SetActiveScene(scene);
        }
    }

    void OnActiveSceneChanged(Scene previousScene, Scene newScene)
    {
        // Unload all scenes that are not the new active scene, except for the new active scene itself
        // Create a list of all currently loaded scenes to check for unloading, since the scene count will change as we unload scenes
        int count = SceneManager.sceneCount;
        var scenes = new List<Scene>();

        for (int i = 0; i < count; i++)
        {
            scenes.Add(SceneManager.GetSceneAt(i));
        }

        foreach (var scene in scenes)
        {
            if (scene != newScene)
            {
                // Unload scenes that are not the new active scene
                SceneManager.UnloadSceneAsync(scene.name);
            }
        }
    }
}