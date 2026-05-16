using UnityEditor.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPreset", menuName = "Scriptable Objects/EnemyPreset")]
public class EnemyPreset : ScriptableObject
{
    public Mesh mesh;
    public GameObject helmet;
    public GameObject rightHandWeapon;
    public GameObject leftHandWeapon;
    public AnimatorOverrideController overrideController;
    public float maxHealth = 30;
    public RarityTier tier = RarityTier.Common;
    public LootTables lootTables;
}
