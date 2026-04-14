using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] int sceneToLoadIndex;

    public void Interact()
    {
        GlobalSceneManager.Instance.LoadSceneTransition(SceneData.Instance[sceneToLoadIndex]);
    }
}