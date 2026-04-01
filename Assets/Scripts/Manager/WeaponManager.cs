using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject currentRightHandWeapon;
    [SerializeField] GameObject currentLeftHandWeapon;

    bool Hidden = false;
    DamageTrigger rightDamageTrigger;
    DamageTrigger leftDamageTrigger;

    private void Start()
    {
        if (currentRightHandWeapon != null)
        {
            rightDamageTrigger = currentRightHandWeapon.GetComponent<DamageTrigger>();
        }
        else
        {
            Debug.Log("Current right hand weapon is not assigned in the inspector.");
        }

        if (currentLeftHandWeapon != null)
        {
            leftDamageTrigger = currentLeftHandWeapon.GetComponent<DamageTrigger>();
        }
        else
        {
            Debug.Log("Current left hand weapon is not assigned in the inspector.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Hidden = !Hidden;
        }

        if (Hidden)
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
            Debug.Log("Activating right damage collider");

            currentRightHandWeapon.GetComponent<Collider>().enabled = true;

            Debug.Log("Collider enabled: " + currentRightHandWeapon.GetComponent<Collider>().enabled);

            rightDamageTrigger.ResetDamage();
        }
    }

    public void DeactivateRightDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            Debug.Log("Deactivating right damage collider");

            currentRightHandWeapon.GetComponent<Collider>().enabled = false;

            Debug.Log("Collider enabled: " + currentRightHandWeapon.GetComponent<Collider>().enabled);
        }
    }

    public void ActivateLeftDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            Debug.Log("Activating left damage collider");

            currentLeftHandWeapon.GetComponent<Collider>().enabled = true;

            Debug.Log("Collider enabled: " + currentRightHandWeapon.GetComponent<Collider>().enabled);

            leftDamageTrigger.ResetDamage();
        }
    }

    public void DeactivateLeftDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            Debug.Log("Deactivating left damage collider");

            currentLeftHandWeapon.GetComponent<Collider>().enabled = false;

            Debug.Log("Collider enabled: " + currentRightHandWeapon.GetComponent<Collider>().enabled);
        }
    }
}