using System.Collections;
using UnityEngine;


// Script made by Henric some random date.
public class Droppable : MonoBehaviour, IPickupable
{
    [SerializeField] private float weight = 5.0f;
    [SerializeField] private float pickUpDelay;
    [SerializeField] private string itemName;

    [SerializeField] private Tier tier = Tier.Common;
    public Tier Tier => tier;

    [SerializeField] private PickableState pickable = PickableState.NotPickable;
    public PickableState Pickable => pickable;

    private Transform playerTransform;

    public float Weight => weight;
    public string Name
    {
        get => itemName;
        set => itemName = value;
    }

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
            playerTransform = player.transform;

        StartCoroutine(WaitForInitialization(pickUpDelay));

    }

    private void OnEnable()
    {
        if (LootManager.instance != null)
            LootManager.instance.RegisterLoot(this);
    }

    private void OnDisable()
    {
        if (LootManager.instance != null)
            LootManager.instance.UnregisterLoot(this);
    }

    IEnumerator WaitForInitialization(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickable = PickableState.Pickable;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by: " + other.name);

        if (other.CompareTag("Player") && pickable == PickableState.Pickable)
        {
            Pickup();
        }
    }

    public void Pickup()
    {
        Debug.Log($"You picked up {itemName}");
        Event_System.instance?.OnLootPickedUp.Invoke();
        Destroy(gameObject);
    }
}