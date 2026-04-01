using UnityEngine;

public class InteractTest : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);
    }
}
