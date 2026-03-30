
using UnityEngine;

public interface IPickupable
{
    public void Pickup();
    public string Name { get; set; }
    public enum PickableState { NotPickable, Pickable}
    public enum Tier { Common, Uncommon, Rare, Epic, Legendary }


    
}
