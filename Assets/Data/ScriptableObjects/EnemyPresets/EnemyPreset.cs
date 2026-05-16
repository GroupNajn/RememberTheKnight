using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPreset", menuName = "Scriptable Objects/EnemyPreset")]
public class EnemyPreset : ScriptableObject
{
    public Mesh mesh;
    public GameObject helmet;
    public GameObject rightHandWeapon;
    public GameObject leftHandWeapon;
    public float maxHealth = 30;
}
