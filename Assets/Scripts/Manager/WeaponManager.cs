using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject currentRightHandWeapon;
    [SerializeField] GameObject currentLeftHandWeapon;

    bool Hidden = true;

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
        if (currentRightHandWeapon != null )
        {
            currentRightHandWeapon.GetComponent<Collider>().enabled = true;
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
        if (currentRightHandWeapon != null )
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = true;
        }
    }

    public void DeactivateLeftDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            currentLeftHandWeapon.GetComponent<Collider>().enabled = false;
        }
    }
}