using UnityEngine;

public class InteractCardSelect: MonoBehaviour, IInteractable
{
    UIManager playerUIManager;

    [field: SerializeField] public string InfoString { get; private set; } = null;
    [field:SerializeField] public string ErrorString { get; private set; } = null;
    public bool IsInteractable { get; set; } = false;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
    }
    public void Interact()
    {
        Debug.Log("Interacted with " + gameObject.name);

        playerUIManager.OpenCardSelectUI();
    }
}
