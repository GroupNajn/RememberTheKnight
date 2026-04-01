using UnityEngine;

public class InteractCrystalBall : MonoBehaviour, IInteractable
{   
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        ThisSceneManager.Instance.LoadNewScene("LockOnCam Implementation");
    }
}
