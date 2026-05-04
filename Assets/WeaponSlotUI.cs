using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] public WeaponData currentWeaponData;
    [SerializeField] public WeaponData lastWeaponData;
    [SerializeField] private Image weaponIcon;

    PlayerWeaponManager weaponManager;

    public void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        weaponManager = player.GetComponent<PlayerWeaponManager>();
        weaponIcon = GetComponent<Image>();

        weaponManager.OnWeaponChanged += UpdateWeaponIcon;

        UpdateWeaponIcon(weaponManager.currentActiveWeaponData);
    }

    private void OnDestroy()
    {
        if (weaponManager != null)
            weaponManager.OnWeaponChanged -= UpdateWeaponIcon;
    }

    void UpdateWeaponIcon(WeaponData newWeapon)
    {
        if (newWeapon == null)
            return;

        weaponIcon.sprite = newWeapon.WeaponIcon;
    }
}
