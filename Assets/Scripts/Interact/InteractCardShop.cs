using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCardShop : MonoBehaviour, IInteractable, IInteractableUIText
{
    [SerializeField] private InteractCameraPreset preset;
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    private UIManager playerUIManager;
    private CardShopUI cardShopUI;
    private ShopBoard board;

    private bool canShowUI = false;

    private InteractCameraHandler interactCameraHandler;
    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        cardShopUI = FindFirstObjectByType<CardShopUI>(FindObjectsInactive.Include);
        board = GetComponent<ShopBoard>();
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

        UIData.InfoText = "[F]: Buy a Card.";
        return UIData;
    }
}