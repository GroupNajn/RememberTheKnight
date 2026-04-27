using UnityEngine;
using UnityEngine.InputSystem;

public class FamilySelectUI : MonoBehaviour
{

    private UIManager uiManager;
    private PlayerInput playerInput;

    void Start()
    {
        playerInput = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
        uiManager = GetComponentInParent<UIManager>();
    }

    void Update()
    {
        
    }
}
