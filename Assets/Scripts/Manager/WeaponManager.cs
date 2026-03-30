using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject currentRightHandWeapon;
    [SerializeField] GameObject currentLeftHandWeapon;

    bool Hidden = false;

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
            Debug.Log("Activating right damage collider");

            currentRightHandWeapon.GetComponent<Collider>().enabled = true;

            Debug.Log("Collider enabled: " + currentRightHandWeapon.GetComponent<Collider>().enabled);
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
        if (currentRightHandWeapon != null )
        {
            Debug.Log("Activating left damage collider");

            currentLeftHandWeapon.GetComponent<Collider>().enabled = true;

            Debug.Log("Collider enabled: " + currentRightHandWeapon.GetComponent<Collider>().enabled);
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