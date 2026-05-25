using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] public WeaponData currentWeaponData;
    [SerializeField] public WeaponData lastWeaponData;
    [SerializeField] private Image weaponIcon;

    [SerializeField] private GameObject unarmed;
    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject swordLarge;

    [SerializeField] private WeaponData unarmedData;
    [SerializeField] private WeaponData swordData;
    [SerializeField] private WeaponData spearData;
    [SerializeField] private WeaponData swordLargeData;

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

        if (newWeapon == unarmedData)
        {
            spear.SetActive(false);
            swordLarge.SetActive(false);
            sword.SetActive(false);
            unarmed.SetActive(true);
        }

        if (newWeapon == swordData)
        {
            unarmed.SetActive(false);
            spear.SetActive(false);
            swordLarge.SetActive(false);
            sword.SetActive(true);
        }

        if (newWeapon == spearData)
        {
            unarmed.SetActive(false);
            sword.SetActive(false);
            swordLarge.SetActive(false);
            spear.SetActive(true);
        }

        if (newWeapon == swordLargeData)
        {
            unarmed.SetActive(false);
            sword.SetActive(false);
            spear.SetActive(false);
            swordLarge.SetActive(true);
        }
    }
}
