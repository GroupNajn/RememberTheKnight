using UnityEngine;
using UnityEngine.SceneManagement;

public class ThisSceneManager : MonoBehaviour
{
    public static ThisSceneManager Instance { get; private set; }

    [field: SerializeField] public Transform PlayerSpawnPosition {  get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void LoadNewScene(string sceneName)
    {
        Debug.Log("LoadNewScene called with sceneName: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }
}