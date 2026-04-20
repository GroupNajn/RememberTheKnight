using UnityEngine;

public class LoadScene : MonoBehaviour
{
    // Used for buttons that don't have a start method or variable to load preload a scene

    [SerializeField] int sceneToLoad;

    private void Start()
    {
        Event_System.instance.OnLoadScenes += OnLoadScenes;
    }

    private void OnEnable()
    {
    }

    private void OnLoadScenes()
    {
        GlobalSceneManager.Instance.LoadScene(SceneData.Instance[sceneToLoad]);
    }
}