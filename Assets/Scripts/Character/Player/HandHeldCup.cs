using UnityEngine;

public class HandHeldCup : MonoBehaviour
{
    // Enables and disables cup when player is drinking or not
    Animator playerAnimator;
    PlayerStates playerStates;
    AnimatorStateInfo currentState;
    public GameObject cup;
    int layerIndex;
    void Start()
    {
        playerAnimator = GetComponentInParent<Animator>();
        playerStates = GetComponentInParent<PlayerStates>();
        layerIndex = playerAnimator.GetLayerIndex("UpperBody");

        
    }

    // Update is called once per frame
    void Update()
    {
        if(cup != null)
        {
            currentState = playerAnimator.GetCurrentAnimatorStateInfo(layerIndex);

            if (currentState.IsTag("Healing"))
            {
                cup.SetActive(true);
                playerStates.SetIsHealing(true);
            }
            else
            {
                cup.SetActive(false);
                playerStates.SetIsHealing(false);
            }
        }
    }
}
