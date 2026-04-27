using System.Transactions;
using UnityEngine;

public class CharacterWeaponManager : MonoBehaviour
{
    [SerializeField] public GameObject currentRightHandWeapon;
    [SerializeField] public GameObject currentLeftHandWeapon;

    protected bool Holsterd = false;
    protected DamageTrigger rightDamageTrigger;
    protected DamageTrigger leftDamageTrigger;

    public WeaponData currentRightWeaponData;
    public WeaponData currentLeftWeaponData;

    public WeaponData currentActiveWeaponData;

    protected CharacterSoundFXManager characterSoundFXManager;
    protected float finalDamage;



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
        if (Holsterd)
        {
            currentLeftHandWeapon.SetActive(false);
            currentRightHandWeapon.SetActive(false);
        }
        else
        {
            currentLeftHandWeapon.SetActive(true);
            currentRightHandWeapon.SetActive(true);
        }


    }

    public virtual void ActivateRightDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            currentRightHandWeapon.GetComponent<Collider>().enabled = true;
            rightDamageTrigger.ResetDamage();
            characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentRightWeaponData.whooshes));

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
            characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentLeftWeaponData.whooshes));

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