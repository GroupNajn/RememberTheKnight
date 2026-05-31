using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractBossCardSelect : MonoBehaviour, IInteractable, IInteractableUIText
{
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
    }

    public void Interact()
    {
        playerUIManager.UIMenuActive = true;
        playerUIManager.cameraTransitioning = true;

        interactCameraHandler.InteractCamSwitch(cameraLookAtTransform, preset);
        StartCoroutine(OpenUI());
    }

    IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(stateDrivenCamera.DefaultBlend.Time);
        playerUIManager.OpenBossCardSelectUI();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        UIData.InfoText = "Choose Court Card";
        return UIData;
    }
}
