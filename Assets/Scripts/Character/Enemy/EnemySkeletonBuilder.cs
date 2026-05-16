using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(EnemyWeaponManager))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(EnemyDamage))]
public class NewBehaviourScript : MonoBehaviour
{
    [SerializeField] List<EnemyPreset> presets;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    EnemyWeaponManager enemyWeaponManager;
    EnemyDamage enemyDamage;
    Transform rightHandWeaponSlot;
    Transform leftHandWeaponSlot;
    Transform helmetSlot;

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
            EnemyPreset selectedPreset = presets[Random.Range(0, presets.Count)];
            if (selectedPreset.helmet != null) Instantiate(selectedPreset.helmet, helmetSlot);
            enemyWeaponManager.currentRightHandWeapon = Instantiate(selectedPreset.rightHandWeapon, rightHandWeaponSlot);
            enemyWeaponManager.currentLeftHandWeapon = Instantiate(selectedPreset.leftHandWeapon, leftHandWeaponSlot);

            enemyWeaponManager.Start();
            skinnedMeshRenderer.sharedMesh = selectedPreset.mesh;
            enemyDamage.MaxHealth = selectedPreset.maxHealth;
            enemyDamage.Health = selectedPreset.maxHealth;

            if (selectedPreset.overrideController)
                GetComponent<Animator>().runtimeAnimatorController = selectedPreset.overrideController;
        }
    }
    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null) return;

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
