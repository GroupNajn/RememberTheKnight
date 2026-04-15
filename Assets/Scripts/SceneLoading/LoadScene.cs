using UnityEngine;

public class LoadScene : MonoBehaviour
{
    // Used for buttons that don't have a start method or variable to load preload a scene

    [SerializeField] int sceneToLoad;

    private void OnEnable()
    {
        GlobalSceneManager.Instance.LoadScene(SceneData.Instance[sceneToLoad]);
    }
}