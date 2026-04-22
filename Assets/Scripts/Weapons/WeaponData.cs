using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public Sprite WeaponIcon;
    public string WeaponName;

    [Header("Weapon Stats")]
    [SerializeField] public float LightDamage = 1f;
    [SerializeField] public float LightChargedDamageBonus = 1f;

    [SerializeField] public float HeavyDamage = 1f;
    [SerializeField] public float HeavyChargedDamage = 1f;

    [Header("SFX")]
    public AudioClip[] whooshes;
}
