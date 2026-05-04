using System.Collections;
using System.Runtime.InteropServices;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCardShop : MonoBehaviour, IInteractable, IInteractableUI
{
    [SerializeField] private InteractCameraPreset preset;
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    private UIManager playerUIManager;
    private CardShopUI cardShopUI;
    private ShopBoard board;

    private bool canShowUI = false;

    private InteractUI_Controller interactUI_Controller;
    private InteractCameraHandler interactCameraHandler;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        cardShopUI = FindFirstObjectByType<CardShopUI>(FindObjectsInactive.Include);
        board = GetComponent<ShopBoard>();
        interactUI_Controller = GetComponent<InteractUI_Controller>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
        stateDrivenCamera = GameObject.FindWithTag("StateDrivenCamera").GetComponent<CinemachineStateDrivenCamera>();
    }
    public void Interact()
    {
        interactCameraHandler.InteractCamSwitch(transform, preset);
        cardShopUI.shopBoard = board;
        StartCoroutine(OpenUI());
    }

    IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(stateDrivenCamera.DefaultBlend.Time);

        playerUIManager.OpenCardShopUI();
        cardShopUI.PopulateSlots();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();

        UIData.InfoText = "Buy a card.";
        return UIData;

    }

    public void ShowUI()
    {
        if (!canShowUI && interactUI_Controller != null) return;

        interactUI_Controller.EnableCanvasObject();

    }

    public void HideUI()
    {

        interactUI_Controller.DisableCanvas();
    }

    public void SetLookedAt(bool value)
    {
        canShowUI = value;

    }
}
