using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    string sceneName;

    private void Start()
    {
        sceneName = SceneData.Instance[sceneToLoadIndex];

        GlobalSceneManager.Instance.LoadScene(sceneName);
    }

    public void Interact()
    {
        GlobalSceneManager.Instance.ActivateSceneTransition(sceneName);
    }
}