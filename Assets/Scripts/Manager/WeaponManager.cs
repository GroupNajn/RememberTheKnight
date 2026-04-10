using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    CharacterSoundFXManager characterSoundFXManager;

    [SerializeField] GameObject currentRightHandWeapon;
    [SerializeField] GameObject currentLeftHandWeapon;

    bool Holsterd = false;
    DamageTrigger rightDamageTrigger;
    DamageTrigger leftDamageTrigger;

    WeaponData currentRightWeaponData;
    WeaponData currentLeftWeaponData;

    private void Start()
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

    private void Update()
    {  
        HolsterCheck();
    }

    private void OnHolster()
    {
        Holsterd = !Holsterd;
    }
    private void HolsterCheck()
    {
        if (Holsterd)
        {
            currentRightHandWeapon.SetActive(false);
            currentLeftHandWeapon.SetActive(false);
        }
        else
        {
            currentRightHandWeapon.SetActive(true);
            currentLeftHandWeapon.SetActive(true);
        }
    }

    public void ActivateRightDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            currentRightHandWeapon.GetComponent<Collider>().enabled = true;
            characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentRightWeaponData.whooshes));
            rightDamageTrigger.ResetDamage();
        }
    }

    public void DeactivateRightDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {

            currentRightHandWeapon.GetComponent<Collider>().enabled = false;

        }
    }

    public void ActivateLeftDamageCollider()
    {
        if (currentLeftHandWeapon != null)
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = true;
            characterSoundFXManager.PlaySoundFX(WorldSoundFXManager.instance.ChooseRandomSFXFromArray(currentLeftWeaponData.whooshes));
            leftDamageTrigger.ResetDamage();
        }
    }

    public void DeactivateLeftDamageCollider()
    {
        if (currentLeftHandWeapon != null)
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = false;
        }
    }
}