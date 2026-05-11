using System.Collections.Generic;
using TMPro;
using Unity.AppUI.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class familyBookUI : AutoSelectFirstButtonOnEnable
{
    private UIManager uiManager;
    private PlayerInput playerInput;
    CardFamily confirmedFamily;
    protected override void OnEnable()
    {
        base.OnEnable();
    }
    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();
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
    }
}
