using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject currentRightHandWeapon;

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
        }
        else
        {
            currentRightHandWeapon.SetActive(true);
        }
    }

    public void ActivateDamageCollider()
    {
        if (currentRightHandWeapon != null )
        {
            currentRightHandWeapon.GetComponent<Collider>().enabled = true;
        }
    }

    public void DeactivateDamageCollider()
    {
        if (currentRightHandWeapon != null)
        {
            currentRightHandWeapon.GetComponent<Collider>().enabled = false;
        }
    }
}
