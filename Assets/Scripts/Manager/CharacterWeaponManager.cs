using FMODUnity;
using UnityEngine;

public class CharacterWeaponManager : MonoBehaviour
{
    [SerializeField] public GameObject currentRightHandWeapon;
    [SerializeField] public GameObject currentLeftHandWeapon;
    [SerializeField] public WeaponData equippedWeapon;

    [SerializeField] public WeaponData currentActiveWeaponData;
    [SerializeField] public WeaponData lastActiveWeaponData;

    [SerializeField] public GameObject rightHandUnarmedWeapon;
    [SerializeField] public GameObject leftHandUnarmedWeapon;
    [SerializeField] public WeaponData unarmedWeaponData;

    [HideInInspector] public DamageTrigger rightDamageTrigger;
    [HideInInspector] public DamageTrigger leftDamageTrigger;
    [HideInInspector] public DamageTrigger rightUnarmedDamageTrigger;
    [HideInInspector] public DamageTrigger leftUnarmedDamageTrigger;
    [HideInInspector] public WeaponData currentRightWeaponData;
    [HideInInspector] public WeaponData currentLeftWeaponData;

    protected bool holsterd = false;

    protected CharacterSoundFXManager characterSoundFXManager;
    protected float finalDamage;

    public bool Holsterd => holsterd;

    public virtual void Start()
    {
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();

        if (currentRightHandWeapon != null)
        {
            rightDamageTrigger = currentRightHandWeapon.GetComponent<DamageTrigger>();
            currentRightWeaponData = currentRightHandWeapon.GetComponent<WeaponStats>().WeaponData;
            currentActiveWeaponData = currentRightWeaponData;
        }

        if (currentLeftHandWeapon != null)
        {
            leftDamageTrigger = currentLeftHandWeapon.GetComponent<DamageTrigger>();
            currentLeftWeaponData = currentLeftHandWeapon.GetComponent<WeaponStats>().WeaponData;
        }

        if (rightHandUnarmedWeapon != null)
        {
            rightUnarmedDamageTrigger = rightHandUnarmedWeapon.GetComponent<DamageTrigger>();
        }

        if (leftHandUnarmedWeapon != null)
        {
            leftUnarmedDamageTrigger = leftHandUnarmedWeapon.GetComponent<DamageTrigger>();
        }
    }

    public virtual void Update()
    {

    }
    public virtual void HolsterCheck()
    {
        if (holsterd)
        {
            currentLeftHandWeapon.SetActive(false);
            currentRightHandWeapon.SetActive(false);

            lastActiveWeaponData = currentActiveWeaponData;
            currentActiveWeaponData = unarmedWeaponData;

            leftHandUnarmedWeapon.SetActive(true);
            rightHandUnarmedWeapon.SetActive(true);
        }
        else
        {
            currentLeftHandWeapon.SetActive(true);
            currentRightHandWeapon.SetActive(true);

            currentActiveWeaponData = lastActiveWeaponData;
            lastActiveWeaponData = unarmedWeaponData;

            leftHandUnarmedWeapon.SetActive(false);
            rightHandUnarmedWeapon.SetActive(false);
        }
    }

    public virtual void ActivateRightDamageCollider()
    {
        if (!holsterd)
        {
            if (currentRightHandWeapon != null)
            {
                var rightCollider = currentRightHandWeapon.GetComponent<Collider>();
                if (rightCollider)
                {
                    rightCollider.enabled = true;
                    rightDamageTrigger.ResetDamage();

                    RuntimeManager.PlayOneShotAttached(currentActiveWeaponData.SwooshEvent, gameObject);

                    currentActiveWeaponData = currentRightWeaponData;
                }
            }
        }
        else
        {
            if (rightHandUnarmedWeapon != null)
            {
                var rightCollider = rightHandUnarmedWeapon.GetComponent<Collider>();
                if (rightCollider)
                {
                    rightCollider.enabled = true;
                    rightUnarmedDamageTrigger.ResetDamage();

                    RuntimeManager.PlayOneShotAttached(unarmedWeaponData.SwooshEvent, gameObject);

                    currentActiveWeaponData = unarmedWeaponData;
                }
            }
        }
    }

    public virtual void DeactivateRightDamageCollider()
    {
        if (!holsterd)
        {
            if (currentRightHandWeapon != null)
            {
                var rightCollider = currentRightHandWeapon.GetComponent<Collider>();
                if (rightCollider) rightCollider.enabled = false;
            }
        }
        else
        {
            if (rightHandUnarmedWeapon != null)
            {
                var rightCollider = rightHandUnarmedWeapon.GetComponent<Collider>();
                if (rightCollider) rightCollider.enabled = false;
            }
        }
    }

    public virtual void ActivateLeftDamageCollider()
    {
        if (!holsterd)
        {
            if (currentLeftHandWeapon != null)
            {
                var leftCollider = currentLeftHandWeapon.GetComponent<Collider>();
                if (leftCollider)
                {
                    leftCollider.enabled = true;
                    leftDamageTrigger.ResetDamage();

                    //characterSoundFXManager.PlayAttackGrunt();
                    //characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentLeftWeaponData.whooshes), 1.5f);
                    RuntimeManager.PlayOneShotAttached(currentActiveWeaponData.SwooshEvent, gameObject);

                    currentActiveWeaponData = currentLeftWeaponData;
                }
            }
        }
        else
        {
            if (leftHandUnarmedWeapon != null)
            {
                var leftCollider = leftHandUnarmedWeapon.GetComponent<Collider>();
                if (leftCollider)
                {
                    leftCollider.enabled = true;
                    leftUnarmedDamageTrigger.ResetDamage();

                    RuntimeManager.PlayOneShotAttached(unarmedWeaponData.SwooshEvent, gameObject);

                    currentActiveWeaponData = unarmedWeaponData;
                }
            }
        }
    }

    public virtual void DeactivateLeftDamageCollider()
    {
        if (!holsterd)
        {
            if (currentLeftHandWeapon != null)
            {
                var leftCollider = currentLeftHandWeapon.GetComponent<Collider>();
                if (leftCollider) leftCollider.enabled = false;
            }
        }
        else
        {
            if (leftHandUnarmedWeapon != null)
            {
                var leftCollider = leftHandUnarmedWeapon.GetComponent<Collider>();
                if (leftCollider) leftCollider.enabled = false;
            }
        }
    }

    public virtual float CalculateFinalDamage(WeaponData weaponData)
    {
        return finalDamage;
    }
}