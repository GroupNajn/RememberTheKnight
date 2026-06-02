using FMODUnity;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class InteractCardShop : MonoBehaviour, IInteractable, IInteractableUIText
{
    /// <summary>
    /// Created by Anton 2026-04-28
    /// Initially created as a component to handle the card shop interaction, which is the component that is attached to the shop board and handles the interaction with the player.
    /// 
    /// Changed by Anton 2026-05-04
    /// Added Enumerator to delay the opening of the UI until the camera transition is finished,
    /// to prevent the UI from opening before the camera has switched to the correct position.
    /// 
    /// Changed by Anton 2026-05-06
    /// Added saved data to the interactable, so that a one time effect can be played the first time the player interacts, and not played again after that.
    /// 
    /// Changed by Anton 2026-05-20
    /// Added logic for setting the shop as bought
    /// so that the player can't interact with the shop after buying a card
    /// </summary>

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
        }

        if (firstTimeEffect != null)
            firstTimeEffect.SetActive(false);

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