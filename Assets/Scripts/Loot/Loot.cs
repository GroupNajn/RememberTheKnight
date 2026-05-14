using System.Collections;
using FMODUnity;

using UnityEngine;


// Script made by Henric some random date.
public abstract class Loot : MonoBehaviour, IPickupable
{
    [SerializeField] protected float weight = 5.0f;
    [SerializeField] protected float pickUpDelay;
    [field: SerializeField] public string itemName { get;  private set;}

    [SerializeField] protected RarityTier tier = RarityTier.Common;
    public RarityTier Tier => tier;

    [SerializeField] protected PickableState pickable = PickableState.NotPickable;

    public EventReference appearEvent;
    public EventReference pickupEvent;





    public PickableState Pickable => pickable;
    protected bool followLogicOverritten = false;
    public bool FollowLogicOverritten => followLogicOverritten;

    protected Transform playerTransform;

    public float Weight => weight;
    public string Name
    {
        get => itemName;
        set => itemName = value;
    }

    protected virtual void  Start()
    {
        RuntimeManager.PlayOneShotAttached(appearEvent, gameObject);

        GameObject player = GameObject.FindWithTag("Player");
        
        if (player != null)
            playerTransform = player.transform;

        StartCoroutine(WaitForInitialization(pickUpDelay));
        if (LootManager.instance != null)
            LootManager.instance.RegisterLoot(this);


    }

    private void OnDestroy()
    {
    }

    //private void OnEnable()
    //{
    //    //if (LootManager.instance != null)
    //    //    LootManager.instance.RegisterLoot(this);
    //}

    //private void OnDisable()
    //{
    //    if (LootManager.instance != null)
    //        LootManager.instance.UnregisterLoot(this);
    //}

    protected IEnumerator WaitForInitialization(float delay)
    {
        yield return new WaitForSeconds(delay);
        pickable = PickableState.Pickable;
    }

    public virtual void Pickup()
    {
        //Debug.Log($"You picked up {itemName}");
        RuntimeManager.PlayOneShotAttached(pickupEvent, playerTransform.gameObject);
        Destroy(gameObject);
        Event_System.instance?.OnLootPickedUp.Invoke(this);
    }
}