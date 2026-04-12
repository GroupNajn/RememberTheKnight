using UnityEngine;
using UnityEngine.InputSystem;

public class playerWeaponManager : CharacterWeaponManager
{
    public override void Start()
    {
        base.Start();
    }
    public override void Update()
    {
        HolsterCheck();
    }

    private void OnHolster(InputValue action)
    {
        Holsterd = !Holsterd;
    }
    
}
