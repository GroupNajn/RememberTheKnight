using UnityEngine;
using UnityEngine.InputSystem;

public class playerWeaponManager : CharacterWeaponManager
{ 
    private void OnHolster(InputValue action)
    {
        Holsterd = !Holsterd;
        HolsterCheck();
    }
}
