using System.Collections.Generic;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.UI;

public class familyBookUI : AutoSelectFirstButtonOnEnable
{
    private UIManager uiManager;
    private InteractCameraHandler interactCameraHandler;

    private PlayerInput playerInput;
    CardFamily confirmedFamily;

    [SerializeField] Sprite cupsIndicator;
    [SerializeField] Sprite swordsIndicator;
    [SerializeField] Sprite pentaclesIndicator;
    [SerializeField] Sprite wandsIndicator;

    [SerializeField] GameObject familyIndicatorImage;

    [SerializeField] private FamilyUI defaultFamily;
    protected override void Awake()
    {
        base.Awake();

        if (defaultFamily != null)
        {
            SelectFamily(defaultFamily);
        }     
        
        uiManager = FindFirstObjectByType<UIManager>();
        interactCameraHandler = FindFirstObjectByType<InteractCameraHandler>();

        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
    }

    public void SelectFamily(FamilyUI selected)
    {
        CardFamily selectedFamily;

        foreach (FamilyUI familyUI in GetComponentsInChildren<FamilyUI>(true))
        {
            // SHOW ALL BUTTONS
            familyUI.gameObject.SetActive(true);

            if (familyUI.infoBox != null)
            {
                familyUI.infoBox.SetActive(false);
            }
        }

        // SETS FAMIILY BUTTON INACTIVE
        selected.gameObject.SetActive(false);
        // SET TEXT ACTIVE
        selected.infoBox.SetActive(true);
        selectedFamily = selected.familyData.cardFamily;
        confirmedFamily = selectedFamily;

        EventSystem.current.SetSelectedGameObject(FirstSelectedButton);

    }
    public void OnSignContract()
    {
        PlayerCollection playerCollection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        if (playerCollection != null)
        {
            playerCollection.SignContract(confirmedFamily);
            CardSystem cardSystem = GameObject.Find("CardSystem").GetComponent<CardSystem>();
        }

        uiManager.CloseFamilySelectUI();
        interactCameraHandler.InteractCamReset();

        familyIndicatorImage.SetActive(true);

        Image image = familyIndicatorImage.GetComponent<Image>();

        switch (confirmedFamily)
        {
            case CardFamily.Cups:
                image.sprite = cupsIndicator;
                break;
            case CardFamily.Swords:
                image.sprite = swordsIndicator;
                break;
            case CardFamily.Pentacles:
                image.sprite = pentaclesIndicator;
                break;
            case CardFamily.Wands:
                image.sprite = wandsIndicator;
                break;
            default:
                image.sprite = null;
                break;
        }

        if (image.sprite == null)
        {
            familyIndicatorImage.SetActive(false);
        }
    }

    public void OnExit()
    {
        uiManager.CloseFamilySelectUI();
        interactCameraHandler.InteractCamReset();
    }
}