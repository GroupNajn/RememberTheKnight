using System.Collections.Generic;
using UnityEngine;

public class InteractBossDoor : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Door Settings")]
    [SerializeField] private GameObject leftDoor;
    [SerializeField] private MeshCollider leftDoorCollider;
    [SerializeField] private GameObject rightDoor;
    [SerializeField] private MeshCollider rightDoorCollider;
    [SerializeField] private float animationDuration = 0.1f;
    [SerializeField] private float rotationAngle = 125f;
    [SerializeField] private bool doorOpen;

    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    private UIManager playerUIManager;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
    }

    public void Interact()
    {
        if (doorOpen)
        {
            AnimateCloseDoor(() => 
            {
                leftDoorCollider.enabled = true;
                rightDoorCollider.enabled = true;
            });
        }
        else
        {
            AnimateOpenDoor(() =>
            {
                leftDoorCollider.enabled = true;
                rightDoorCollider.enabled = true;
            });
        }

        doorOpen = !doorOpen;
    }

    public void AnimateOpenDoor(System.Action onComplete = null)
    {
        leftDoorCollider.enabled = false;
        rightDoorCollider.enabled = false;
        LeanTween.rotateAroundLocal(leftDoor, Vector3.up, -rotationAngle, animationDuration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.rotateAroundLocal(rightDoor, Vector3.up, rotationAngle, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => onComplete?.Invoke());
    }

    public void AnimateCloseDoor(System.Action onComplete = null)
    {
        leftDoorCollider.enabled = false;
        rightDoorCollider.enabled = false;
        LeanTween.rotateAroundLocal(leftDoor, Vector3.up, rotationAngle, animationDuration).setEase(LeanTweenType.easeInOutQuad);
        LeanTween.rotateAroundLocal(rightDoor, Vector3.up, -rotationAngle, animationDuration).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() => onComplete?.Invoke());
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "Open Door";
        return UIData;
    }
}
