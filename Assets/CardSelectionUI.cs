using UnityEngine;
using UnityEngine.InputSystem;

public class CardSelectionUI : MonoBehaviour
{
    PlayerInput playerInput;
    PlayerUIManager playerUIManager;

    private void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        playerUIManager = GetComponentInParent<PlayerUIManager>();

        playerUIManager.CloseCardSelectUI(); // Ensure the card selection UI is closed at the start
    }




}
