using UnityEngine;

public class CharacterWeaponManager : MonoBehaviour
{


    [SerializeField] GameObject currentRightHandWeapon;
    [SerializeField] GameObject currentLeftHandWeapon;

    protected bool Holsterd = false;
    protected DamageTrigger rightDamageTrigger;
    protected DamageTrigger leftDamageTrigger;

    protected WeaponData currentRightWeaponData;
    protected WeaponData currentLeftWeaponData;
    
    protected CharacterSoundFXManager characterSoundFXManager;

    public virtual void Start()
    {
        characterSoundFXManager = GetComponent<CharacterSoundFXManager>();

        if (currentRightHandWeapon != null)
        {
            rightDamageTrigger = currentRightHandWeapon.GetComponent<DamageTrigger>();
            currentRightWeaponData = currentRightHandWeapon.GetComponent<WeaponStats>().WeaponData;
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
        }
    }

    public virtual void DeactivateLeftDamageCollider()
    {
        if (currentLeftHandWeapon != null)
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = false;
        }
    }
}