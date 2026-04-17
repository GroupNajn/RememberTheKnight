using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    public string InfoString { get; private set; } = null;
    public void Interact()
    {
        GlobalSceneManager.Instance.LoadSceneTransition(SceneData.Instance[sceneToLoadIndex]);
    }
}