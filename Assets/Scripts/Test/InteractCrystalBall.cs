using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{
    [SerializeField] string sceneToLoad;

    private void Start()
    {
        ThisSceneManager.Instance.LoadScene(sceneToLoad);
    }

    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        ThisSceneManager.Instance.ActivateScene(sceneToLoad);
    }
}
