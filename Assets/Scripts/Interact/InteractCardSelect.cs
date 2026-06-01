using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCardSelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    /// <summary>
    /// Changed by Anton 2026-05-04
    /// Added Enumerator to delay the opening of the UI until the camera transition is finished
    /// 
    /// Changed by Anton 2026-05-06
    /// Added saved data to the interactable, so that a one time effect can be played the first time the player interacts, and not played again after that.
    /// </summary>
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    private UIManager playerUIManager;
    [SerializeField] private InteractCameraPreset preset;
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField] private Transform cameraLookAtTransform;
    private InteractCameraHandler interactCameraHandler;

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
        stateDrivenCamera = GameObject.FindWithTag("StateDrivenCamera").GetComponent<CinemachineStateDrivenCamera>();

        if (PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
    }

    public void Interact()
    {
        if (!PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            PlayerPrefsSaveSystem.SetSaveState(interactableID);

            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }

        playerUIManager.UIMenuActive = true;
        playerUIManager.cameraTransitioning = true;

        interactCameraHandler.InteractCamSwitch(cameraLookAtTransform, preset);
        StartCoroutine(OpenUI());
    }

    IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(stateDrivenCamera.DefaultBlend.Time);

        playerUIManager.OpenCardSelectUI();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "Choose Minor Arcana.";
        return UIData;
    }
}