using System.Transactions;
using UnityEngine;

public class CharacterWeaponManager : MonoBehaviour
{
    [SerializeField] public GameObject currentRightHandWeapon;
    [SerializeField] public GameObject currentLeftHandWeapon;

    protected bool holsterd = false;
    public DamageTrigger rightDamageTrigger;
    public DamageTrigger leftDamageTrigger;

    public WeaponData currentRightWeaponData;
    public WeaponData currentLeftWeaponData;

    public WeaponData currentActiveWeaponData;

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
            Debug.Log($"Activating right damage collider on object {currentRightHandWeapon.name}");
            currentRightHandWeapon.GetComponent<Collider>().enabled = true;
            Debug.Log(currentRightHandWeapon.GetComponent<Collider>().gameObject.name);
            rightDamageTrigger.ResetDamage();

            characterSoundFXManager.PlayAttackGrunt();
            characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentRightWeaponData.whooshes),1.5f);

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

            characterSoundFXManager.PlayAttackGrunt();
            characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentLeftWeaponData.whooshes), 1.5f);

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