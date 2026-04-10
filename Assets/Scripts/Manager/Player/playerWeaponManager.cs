using UnityEngine;

public class playerWeaponManager : CharacterWeaponManager
{
    public override void Update()
    {
        HolsterCheck();
    }

    private void OnHolster()
    {
        Holsterd = !Holsterd;
    }
    
}
