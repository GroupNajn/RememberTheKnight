using UnityEngine;
using FMODUnity;


[CreateAssetMenu(menuName = "Weapons/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public Sprite WeaponIcon;
    public string WeaponName;

    [Header("One Hand Weapon Stats")]
    [SerializeField] public float LightDamage = 1f;
    [SerializeField] public float LightChargedDamageBonus = 1f;

    [SerializeField] public float HeavyDamage = 1f;
    [SerializeField] public float HeavyChargedDamage = 1f;
    [SerializeField] public float ActionSpeed = 1f;
    //Two Handed Weapon Stats
    [Header("Two Handed Weapon Stats")]

    [SerializeField] public float ThLightDamage = 1f;
    [SerializeField] public float ThLightChargedDamageBonus = 1f;

    [SerializeField] public float ThHeavyDamage = 1f;
    [SerializeField] public float ThHeavyChargedDamage = 1f;
    [SerializeField] public float ThActionSpeed = 1f;

    [Header("SFX")]
    public EventReference SwooshEvent;
    public EventReference HitEvent;


    [Header("Animator")]
    public AnimatorOverrideController OneHandedWeaponAnimator;
    public AnimatorOverrideController TwoHandedWeaponAnimator;
}
