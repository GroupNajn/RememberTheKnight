using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FamilySelectUI : MonoBehaviour
{

    private UIManager uiManager;
    private PlayerInput playerInput;
    CardFamily confirmedFamily;

    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();
    }

    public void SelectFamily(FamilyUI selected)
    {
        CardFamily selectedFamily;

        foreach (FamilyUI familyUI in GetComponentsInChildren<FamilyUI>())
        {
            if (familyUI.infoBox != null)
            {
                familyUI.infoBox.SetActive(false);
            }
        }

        selected.infoBox.SetActive(true);
        selectedFamily = selected.familyData.cardFamily;
        confirmedFamily = selectedFamily;

    }

    public void OnSignContract()
    {
        PlayerCollection playerCollection = GameObject.Find("Player").GetComponent<PlayerCollection>(); 
        if(playerCollection != null)
        {
            playerCollection.SignContract(confirmedFamily);
        }

    }

    
}
