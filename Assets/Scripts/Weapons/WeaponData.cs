using UnityEngine;

[CreateAssetMenu(menuName = "Weapons/Weapon Data")]

public class WeaponData : ScriptableObject
{
    public Sprite WeaponIcon;
    public string WeaponName;

    [Header("Weapon Stats")]
    [SerializeField] public float BaseDamage = 1f;

    [Header("SFX")]
    public AudioClip[] whooshes;
}
