using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Animator))]
public class EnemyWeaponManager : CharacterWeaponManager
{
    private static readonly int IsHeavyHash = Animator.StringToHash("IsHeavy");
    private Animator animator;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }
    public new DamageInfo CalculateFinalDamage(WeaponData weaponData)
    {
        // finalDamage = weaponData.base + weapondaata.charged + damgemodifier 
        DamageInfo damageInfo = new();
        damageInfo.SetIsCrit(false);
        finalDamage = animator.GetBool(IsHeavyHash) ? weaponData.HeavyDamage : weaponData.LightDamage;
        damageInfo.SetDamageAmount(finalDamage);

        return damageInfo;
    }

}
