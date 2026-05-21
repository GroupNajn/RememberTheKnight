using UnityEngine;

public class LootPickupTrigger : MonoBehaviour
{
    private Loot loot;

    private void Awake()
    {
        loot = GetComponentInParent<Loot>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (loot.Pickable == PickableState.Pickable)
            {
                loot.Pickup();
            }
        }
    }
}