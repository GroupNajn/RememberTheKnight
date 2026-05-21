using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyWeaponManager : CharacterWeaponManager
{
    [SerializeField] float damageModifierPercentagePerLevel = 1.2f;
    float finalModifier = 1f;

    private static readonly int IsHeavyHash = Animator.StringToHash("IsHeavy");
    private Animator animator;
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public void SetDamageModifier(int level)
    {
        finalModifier = Mathf.Pow(damageModifierPercentagePerLevel, level);
    }

    public new DamageInfo CalculateFinalDamage(WeaponData weaponData)
    {
        DamageInfo damageInfo = new();
        damageInfo.SetIsCrit(false);
        finalDamage = animator.GetBool(IsHeavyHash) ? weaponData.HeavyDamage : weaponData.LightDamage;
        damageInfo.SetDamageAmount(finalDamage * finalModifier);

        return damageInfo;
    }
}
