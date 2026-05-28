using FMODUnity;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCardShop : MonoBehaviour, IInteractable, IInteractableUIText
{
    [Header("Saved Data")]
    [SerializeField] private string interactableID;
    [SerializeField] private GameObject firstTimeEffect;

    [SerializeField] private bool hasBoughtCard = false;
    public bool HasBoughtCard => hasBoughtCard;

    [SerializeField] private InteractCameraPreset preset;
    [SerializeField] private CinemachineStateDrivenCamera stateDrivenCamera;
    [SerializeField] private Transform cameraLookAtTransform;
    private InteractCameraHandler interactCameraHandler;

    private UIManager playerUIManager;
    private CardShopUI cardShopUI;
    private ShopBoard board;

    [Header("SFX")]
   
    [SerializeField] private EventReference igniteLighterCardEvent;

    public void SetBought()
    {
        hasBoughtCard = true;
    }

    void Start()
    {
        playerUIManager = FindFirstObjectByType<UIManager>();
        cardShopUI = FindFirstObjectByType<CardShopUI>(FindObjectsInactive.Include);
        board = GetComponent<ShopBoard>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();
        stateDrivenCamera = GameObject.FindWithTag("StateDrivenCamera").GetComponent<CinemachineStateDrivenCamera>();

        PlayerPrefs.DeleteAll(); // Remove this line after testing to keep player progress

        if (PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }
    }
    public void Interact()
    {
        if (hasBoughtCard)
            return;

        if (!PlayerPrefsSaveSystem.HasInteracted(interactableID))
        {
            PlayerPrefsSaveSystem.SetSaveState(interactableID);

            if (firstTimeEffect != null)
                firstTimeEffect.SetActive(false);
        }

        playerUIManager.UIMenuActive = true;
        playerUIManager.cameraTransitioning = true;

        interactCameraHandler.InteractCamSwitch(transform, preset);
        cardShopUI.shopBoard = board;
        cardShopUI.interactCardShop = this;
        StartCoroutine(OpenUI());
    }

    IEnumerator OpenUI()
    {
        yield return new WaitForSeconds(stateDrivenCamera.DefaultBlend.Time);
        RuntimeManager.PlayOneShot(igniteLighterCardEvent);
        playerUIManager.OpenCardShopUI();
        cardShopUI.PopulateSlots();
    }

    public InteractableUIData GetUIData()
    {
        var UIData = new InteractableUIData();
        UIData.CanInteract = true;

        if (hasBoughtCard)
        {
            UIData.CanInteract = false;
            UIData.InfoText = "Card already purchased.";
            return UIData;
        }

        UIData.InfoText = "Buy a Card.";
        return UIData;
    }
}