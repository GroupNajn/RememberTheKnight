using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class FamilySelectUI : MonoBehaviour
{

    private UIManager uiManager;
    private PlayerInput playerInput;

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

    }
}
