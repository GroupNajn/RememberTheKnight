using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] string sceneToLoad;

    public void Interact()
    {
        GlobalSceneManager.Instance.LoadScene(sceneToLoad);
    }
}