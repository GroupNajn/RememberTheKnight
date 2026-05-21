using UnityEngine;
using UnityEngine.Rendering;

public class HealingSoul : Soul, IPickupable
{
   

    private int healingChargeAmount = 10; // = half a use. 
    public int HealingChargeAmount
    {
        get=> healingChargeAmount;
    }


}
