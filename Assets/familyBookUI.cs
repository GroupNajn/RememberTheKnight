using System.Collections.Generic;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class familyBookUI : AutoSelectFirstButtonOnEnable
{
    private UIManager uiManager;
    private InteractCameraHandler interactCameraHandler;

    private PlayerInput playerInput;
    CardFamily confirmedFamily;


    [SerializeField] private FamilyUI defaultFamily;

    protected override void OnEnable()
    {
        base.OnEnable();

        if(defaultFamily != null)
        {
            SelectFamily(defaultFamily);
        }
    }
    protected override void Start()
    {
        base.Start();

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

        // SETS TAB BUTTON INACTIVE
        selected.gameObject.SetActive(false);
        // SET TEXT ACTIVE
        selected.infoBox.SetActive(true);
        selectedFamily = selected.familyData.cardFamily;
        confirmedFamily = selectedFamily;

    }
    public void OnSignContract()
    {
        PlayerCollection playerCollection = GameObject.Find("Player").GetComponent<PlayerCollection>();
        if (playerCollection != null)
        {
            playerCollection.SignContract(confirmedFamily);
            CardSystem cardSystem = GameObject.Find("CardSystem").GetComponent<CardSystem>();
            cardSystem.UnlockCardsAfterSigningContract(playerCollection.playerContract);
        }

        uiManager.CloseFamilySelectUI();
        interactCameraHandler.InteractCamReset();
    }
}
