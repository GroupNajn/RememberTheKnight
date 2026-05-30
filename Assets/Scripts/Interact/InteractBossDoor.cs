using FMODUnity;
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

    [SerializeField] private bool isAnimating;
    [SerializeField] private bool doorOpen;

    [Header("Saved Data")]
    [SerializeField] private GameData gameData;
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    [Header("SFX")]
    [SerializeField] EventReference openEvent;
    [SerializeField] EventReference closeEvent;


    private UIManager playerUIManager;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        gameData = FindFirstObjectByType<GameData>();
    }

    public void Interact()
    {
        // uncoment for door to open without boss defeated
       // gameData.GameCompleted = true; 

        if (!gameData.GameCompleted)
            return;

        if (isAnimating)
            return;

        isAnimating = true;

        if (doorOpen)
        {
            RuntimeManager.PlayOneShot(closeEvent);
            AnimateCloseDoor(() => 
            {
                leftDoorCollider.enabled = true;
                rightDoorCollider.enabled = true;
                isAnimating = false;
            });
        }
        else
        {
            AnimateOpenDoor(() =>
            {
                RuntimeManager.PlayOneShot(openEvent);

                leftDoorCollider.enabled = false;
                rightDoorCollider.enabled = false;
                isAnimating = false;
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

        if (!gameData.GameCompleted)
        {
            UIData.CanInteract = false;
            UIData.InfoText = "A memory remains unclaimed";
            return UIData;
        }

        UIData.InfoText = "Open Door";
        return UIData;
    }
}
