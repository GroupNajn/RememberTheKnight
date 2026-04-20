using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    public bool IsInteractable { get;  set; } = false;
    [field:SerializeField] public string InfoString { get; private set; } = null;

    [field: SerializeField] public string ErrorString { get; private set; } = null;
    public void Interact()
    {
        GlobalSceneManager.Instance.LoadSceneTransition(SceneData.Instance[sceneToLoadIndex]);
    }
}