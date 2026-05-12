using FMODUnity;
using System.Transactions;
using UnityEngine;

public class CharacterWeaponManager : MonoBehaviour
{
    [SerializeField] public GameObject currentRightHandWeapon;
    [SerializeField] public GameObject currentLeftHandWeapon;
    [SerializeField] public WeaponData equippedWeapon;

    [SerializeField] public WeaponData currentActiveWeaponData;
    [SerializeField] public WeaponData lastActiveWeaponData; 

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
        else
        {
            Debug.Log("Current right hand weapon is not assigned in the inspector.");
        }

        if (currentLeftHandWeapon != null)
        {
            leftDamageTrigger = currentLeftHandWeapon.GetComponent<DamageTrigger>();
            currentLeftWeaponData = currentLeftHandWeapon.GetComponent<WeaponStats>().WeaponData;
        }
        else
        {
            Debug.Log("Current left hand weapon is not assigned in the inspector.");
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
        }
        else
        {
            currentLeftHandWeapon.SetActive(true);
            currentRightHandWeapon.SetActive(true);

            currentActiveWeaponData = lastActiveWeaponData;
            lastActiveWeaponData = unarmedWeaponData;
        }


    }

    public virtual void ActivateRightDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
           // Debug.Log($"Activating right damage collider on object {currentRightHandWeapon.name}");
            currentRightHandWeapon.GetComponent<Collider>().enabled = true;
            //Debug.Log(currentRightHandWeapon.GetComponent<Collider>().gameObject.name);
            rightDamageTrigger.ResetDamage();

           // characterSoundFXManager.PlayAttackGrunt();
            //characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentRightWeaponData.whooshes),1.5f);
            RuntimeManager.PlayOneShotAttached(currentActiveWeaponData.SwooshEvent, gameObject);

            currentActiveWeaponData = currentRightWeaponData;
        }
    }

    public virtual void DeactivateRightDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            currentRightHandWeapon.GetComponent<Collider>().enabled = false;

        }
    }

    public virtual void ActivateLeftDamageCollider()
    {
        if (currentLeftHandWeapon != null)
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = true;
            leftDamageTrigger.ResetDamage();

            //characterSoundFXManager.PlayAttackGrunt();
            //characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentLeftWeaponData.whooshes), 1.5f);
            RuntimeManager.PlayOneShotAttached(currentActiveWeaponData.SwooshEvent, gameObject);

            currentActiveWeaponData = currentLeftWeaponData;
        }
    }

    public virtual void DeactivateLeftDamageCollider()
    {
        if (currentLeftHandWeapon != null)
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = false;

        }
    }

    public virtual float CalculateFinalDamage(WeaponData weaponData)
    {
        return finalDamage;
    }
}