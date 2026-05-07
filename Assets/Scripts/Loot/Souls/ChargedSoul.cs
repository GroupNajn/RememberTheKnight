using UnityEngine;
using UnityEngine.Rendering;

public class ChargedSoul : Soul, IPickupable
{
    private int chargeAmount = 10; // = half a use. 
    public int ChargeAmount
    {
        get=> chargeAmount;
    }


}
