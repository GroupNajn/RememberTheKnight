using FMODUnity;
using UnityEngine;

public class CharacterWeaponManager : MonoBehaviour
{
    [SerializeField] public GameObject currentRightHandWeapon;
    [SerializeField] public GameObject currentLeftHandWeapon;
    [SerializeField] public WeaponData equippedWeapon;

    [SerializeField] public WeaponData currentActiveWeaponData;
    [SerializeField] public WeaponData lastActiveWeaponData;

    [SerializeField] public GameObject rightHandWeaponUnarmedWeapon;
    [SerializeField] public GameObject leftHandWeaponUnarmedWeapon;
    [SerializeField] public WeaponData unarmedWeaponData;

    [HideInInspector] public DamageTrigger rightDamageTrigger;
    [HideInInspector] public DamageTrigger leftDamageTrigger;
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

            leftHandWeaponUnarmedWeapon.SetActive(true);
            rightHandWeaponUnarmedWeapon.SetActive(true);
        }
        else
        {
            currentLeftHandWeapon.SetActive(true);
            currentRightHandWeapon.SetActive(true);

            currentActiveWeaponData = lastActiveWeaponData;
            lastActiveWeaponData = unarmedWeaponData;

            leftHandWeaponUnarmedWeapon.SetActive(false);
            rightHandWeaponUnarmedWeapon.SetActive(false);
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
            Debug.Log("Holstered");
            if (rightHandWeaponUnarmedWeapon != null)
            {
                Debug.Log("rightHandWeaponUnarmedWeapon");
                var rightCollider = rightHandWeaponUnarmedWeapon.GetComponent<Collider>();
                if (rightCollider)
                {
                    Debug.Log("Right collider found");
                    rightCollider.enabled = true;
                    Debug.Log("Right collider enabled: " + rightCollider.enabled);
                    rightDamageTrigger.ResetDamage();

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
            if (rightHandWeaponUnarmedWeapon != null)
            {
                var rightCollider = rightHandWeaponUnarmedWeapon.GetComponent<Collider>();
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
            Debug.Log("Holstered");
            if (leftHandWeaponUnarmedWeapon != null)
            {
                Debug.Log("leftHandWeaponUnarmedWeapon is not null");
                var leftCollider = leftHandWeaponUnarmedWeapon.GetComponent<Collider>();
                if (leftCollider)
                {
                    Debug.Log("Left collider found");
                    leftCollider.enabled = true;
                    Debug.Log("Left collider enabled: " + leftCollider.enabled);
                    leftDamageTrigger.ResetDamage();

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
            if (leftHandWeaponUnarmedWeapon != null)
            {
                var leftCollider = leftHandWeaponUnarmedWeapon.GetComponent<Collider>();
                if (leftCollider) leftCollider.enabled = false;
            }
        }
    }

    public virtual float CalculateFinalDamage(WeaponData weaponData)
    {
        return finalDamage;
    }
}