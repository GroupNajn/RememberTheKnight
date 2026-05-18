using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] public WeaponData currentWeaponData;
    [SerializeField] public WeaponData lastWeaponData;
    [SerializeField] private Image weaponIcon;

    [SerializeField] private GameObject sword;
    [SerializeField] private GameObject spear;
    [SerializeField] private GameObject swordLarge;

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

        weaponIcon.sprite = newWeapon.WeaponIcon;

        //if (newWeapon == swordData)
        //{
        //    spear.SetActive(false);
        //    swordLarge.SetActive(false);
        //    sword.SetActive(true);
        //}

        //if (newWeapon == spearData)
        //{
        //    sword.SetActive(false);
        //    swordLarge.SetActive(false);
        //    spear.SetActive(true);
        //}

        //if (newWeapon == swordLargeData)
        //{
        //    sword.SetActive(false);
        //    spear.SetActive(false);
        //    swordLarge.SetActive(true);
        //}

        // Uncomment this when you want to use it!
    }
}
