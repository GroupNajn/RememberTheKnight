using System.Collections;
using System.Linq;
using Unity.Behavior;
using UnityEditor.Animations;
using UnityEngine;
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BossVFXManager))]
[RequireComponent(typeof(EnemyWeaponManager))]

[RequireComponent(typeof(BehaviorGraphAgent))]
public class BossUnarmedHandler : MonoBehaviour
{
    private static readonly int IsDisarmedHash = Animator.StringToHash("IsDisarmed");
    [SerializeField] GameObject mainRightWeapon;
    [SerializeField] GameObject mainLeftWeapon;
    [SerializeField] GameObject unarmedRightWeapon;
    [SerializeField] GameObject unarmedLeftWeapon;

    Transform airOriginRight;
    Transform unarmedAirOriginRight;
    Transform unarmedAirOriginLeft;

    [SerializeField] AnimatorController mainAnimationController;
    [SerializeField] AnimatorOverrideController unarmedController;
    Animator animator;
    BlackboardVariable<bool> isDisarmed;
    BossVFXManager bossVFX;
    EnemyWeaponManager enemyWeaponManager;



    void Start()
    {
        animator = GetComponent<Animator>();
        enemyWeaponManager = GetComponent<EnemyWeaponManager>();
        bossVFX = GetComponent<BossVFXManager>();
        if (GetComponent<BehaviorGraphAgent>().BlackboardReference.GetVariable("isDisarmed", out isDisarmed)) { }
        airOriginRight = mainRightWeapon.GetComponentsInChildren<Transform>().FirstOrDefault(transform => transform.name == "ParticleSlot");
        unarmedAirOriginRight = unarmedRightWeapon.GetComponentsInChildren<Transform>().FirstOrDefault(transform => transform.name == "ParticleSlot");
        unarmedAirOriginLeft = unarmedLeftWeapon.GetComponentsInChildren<Transform>().FirstOrDefault(transform => transform.name == "ParticleSlot");
    }


    public void OnDisarm()
    {
        mainRightWeapon.SetActive(false);
        mainLeftWeapon.SetActive(false);
        unarmedRightWeapon.SetActive(true);
        unarmedLeftWeapon.SetActive(true);
        enemyWeaponManager.currentRightHandWeapon = unarmedRightWeapon;
        enemyWeaponManager.currentLeftHandWeapon = unarmedLeftWeapon;
        enemyWeaponManager.Start();
        bossVFX.airOriginRight = unarmedAirOriginRight;
        bossVFX.airOriginLeft = unarmedAirOriginLeft;
        animator.SetBool(IsDisarmedHash, true);
        animator.runtimeAnimatorController = unarmedController;
        isDisarmed.Value = true;
        StartCoroutine(Rearm());
    }

    IEnumerator Rearm()
    {
        yield return new WaitForSeconds(20);
        mainRightWeapon.SetActive(true);
        mainLeftWeapon.SetActive(true);
        unarmedRightWeapon.SetActive(false);
        unarmedLeftWeapon.SetActive(false);
        enemyWeaponManager.currentRightHandWeapon = mainRightWeapon;
        enemyWeaponManager.currentLeftHandWeapon = mainLeftWeapon;
        enemyWeaponManager.Start();
        bossVFX.airOriginRight = airOriginRight;
        bossVFX.airOriginLeft = null;
        bossVFX.airOriginLeft = unarmedAirOriginLeft;
        animator.runtimeAnimatorController = mainAnimationController;
        animator.SetBool(IsDisarmedHash, false);
        isDisarmed.Value = false;
    }
}
