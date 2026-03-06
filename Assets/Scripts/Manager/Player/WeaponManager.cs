using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] GameObject RightWeaponSlot;

    bool Hidden = true;

    private void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Hidden = !Hidden;
        }

        if (Hidden)
        {
            RightWeaponSlot.SetActive(false);
        }
        else
        {
            RightWeaponSlot.SetActive(true);
        }
    }
}
