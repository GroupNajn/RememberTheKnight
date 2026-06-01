using UnityEngine;

public class PlayerStepHandler : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private LayerMask enemyLayer;

    private float originalStepOffset;
    private int enemyContacts;
    void Start()
    {
        
    }

    private void Awake()
    {
        originalStepOffset = controller.stepOffset;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if((( 1 << hit.gameObject.layer) & enemyLayer) != 0)
        {
            controller.stepOffset = 0f;
            enemyContacts = 2;
        }
    }

    private void LateUpdate()
    {
        if(enemyContacts > 0)
        {
            enemyContacts--;
        }
        else
        {
            controller.stepOffset = originalStepOffset;
        }
    }

    void Update()
    {
        
    }
}
