using UnityEngine;
using UnityEngine.Rendering;

public class ChargedSoul : Soul, IPickupable
{
    private int chargeAmount = 5; // = half a use. 
    public int ChargeAmount
    {
        get=> chargeAmount;
    }


}
