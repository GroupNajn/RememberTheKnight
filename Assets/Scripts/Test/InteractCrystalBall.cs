using UnityEditor.SearchService;
using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] string sceneToLoad;
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        ThisSceneManager.Instance.LoadNewScene(sceneToLoad);
    }
}
