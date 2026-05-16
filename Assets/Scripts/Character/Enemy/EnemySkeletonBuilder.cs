using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(EnemyWeaponManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyDamage))]
[RequireComponent(typeof(EnemyLootProfile))]
public class EnemySkeletonBuilder : MonoBehaviour
{
    [SerializeField] List<EnemyPreset> presets;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    EnemyWeaponManager enemyWeaponManager;
    EnemyDamage enemyDamage;
    Transform rightHandWeaponSlot;
    Transform leftHandWeaponSlot;
    Transform helmetSlot;

    private struct WeightedPreset
    {
        public EnemyPreset Preset;
        public int Weight;

    }

    void Start()
    {
        GetComponentsInChildren<Transform>().ToList().ForEach(transform =>
        {
            switch (transform.name)
            {
                case "RightWeaponSlot": rightHandWeaponSlot = transform; break;
                case "LeftWeaponSlot": leftHandWeaponSlot = transform; break;
                case "HelmetSlot": helmetSlot = transform; break;
            }
        });
        enemyWeaponManager = GetComponent<EnemyWeaponManager>();
        enemyDamage = GetComponent<EnemyDamage>();
        if (presets.Count > 0)
        {
            int totalWeight = 0;
            List<WeightedPreset> weightedPresets = new(presets.Count);
            foreach (var preset in presets)
            {
                weightedPresets.Add(new() { Preset = preset, Weight = preset.weight });
                totalWeight += preset.weight;
            }

            int randomValue = Random.Range(0, totalWeight);
            int currentWeight = 0;
            EnemyPreset selectedPreset = presets[0];
            foreach (var weightedPreset in weightedPresets)
            {
                currentWeight += weightedPreset.Weight;
                if (currentWeight < randomValue)
                {
                    selectedPreset = weightedPreset.Preset;
                    break;
                }
            }

            if (selectedPreset.helmet != null) Instantiate(selectedPreset.helmet, helmetSlot);
            enemyWeaponManager.currentRightHandWeapon = Instantiate(selectedPreset.rightHandWeapon, rightHandWeaponSlot);
            enemyWeaponManager.currentLeftHandWeapon = Instantiate(selectedPreset.leftHandWeapon, leftHandWeaponSlot);

            enemyWeaponManager.Start();
            skinnedMeshRenderer.sharedMesh = selectedPreset.mesh;
            enemyDamage.MaxHealth = selectedPreset.maxHealth;
            enemyDamage.Health = selectedPreset.maxHealth;

            if (selectedPreset.overrideController)
                GetComponent<Animator>().runtimeAnimatorController = selectedPreset.overrideController;

            var enemyLootProfile = GetComponent<EnemyLootProfile>();
            enemyLootProfile.allowedFamiles = selectedPreset.lootTables;
            enemyLootProfile.baseRarity = selectedPreset.tier;
        }
    }
}
