using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCardSelect : MonoBehaviour, IInteractable, IInteractableUIText
{
    [SerializeField] private InteractCameraPreset preset;
    private UIManager playerUIManager;
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;

    private InteractCameraHandler interactCameraHandler;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
        stateDrivenCamera = GameObject.FindWithTag("StateDrivenCamera").GetComponent<CinemachineStateDrivenCamera>();
    }

    public void Interact()
    {
        interactCameraHandler.InteractCamSwitch(transform, preset);
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

        UIData.InfoText = "[F]: Choose Minor Arcana.";
        return UIData;
    }
}