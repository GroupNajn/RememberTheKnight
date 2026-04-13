using UnityEngine;

public class InteractHealingFountain : MonoBehaviour, IInteractable
{
    PlayerStats playerStats;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    public void Interact()
    {
        playerStats.Heal(25f);
    }

}
