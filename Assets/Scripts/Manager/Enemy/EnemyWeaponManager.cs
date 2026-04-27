using UnityEngine;

public class EnemyWeaponManager : CharacterWeaponManager
{
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        // IF ENEMY SHOULD BE ABLE TO HOLSTER WEAPON, IMPLEMENT HOLSTER CHECK HERE
    }

    public override float CalculateFinalDamage(WeaponData weaponData)
    {
        // finalDamage = weaponData.base + weapondaata.charged + damgemodifier 
        
        finalDamage = weaponData.LightDamage;

        return finalDamage;
    }
}
