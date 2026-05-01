using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using TMPro;
using UnityEngine.UI;

public class WeaponSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public List<GameObject> Weapons;
   
    int currentWeaponIndex = 0;
    PlayerWeaponManager playerWeaponManager;
    Animator playerAnimator;

    Image weaponIconImage;

    void Start()
    {
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        playerAnimator = GetComponent<Animator>();
        weaponIconImage = GameObject.FindGameObjectWithTag("WeaponIconImage")?.GetComponent<Image>();
        SelectWeapon();
    }

    public void OnCycle()
    {
       // SelectWeapon();
    }
    public void SelectWeapon()
    {
        currentWeaponIndex = currentWeaponIndex % Weapons.Count; // Wrap around the index

        if (playerWeaponManager.Holsterd)
        {
            playerWeaponManager.OnHolster(null);
        }

        // Deactivate all weapons
        foreach (var weapon in Weapons)
        {
            weapon.SetActive(false);
        }
        Weapons[currentWeaponIndex].SetActive(true); // activate the current weapon
        playerWeaponManager.currentRightHandWeapon = Weapons[currentWeaponIndex]; // weapon gameobject
        playerWeaponManager.currentActiveWeaponData = Weapons[currentWeaponIndex].GetComponent<WeaponStats>().WeaponData; // active weapon data
        playerWeaponManager.currentRightWeaponData = Weapons[currentWeaponIndex].GetComponent<WeaponStats>().WeaponData; // weapon data

        playerWeaponManager.rightDamageTrigger = Weapons[currentWeaponIndex].GetComponent<DamageTrigger>(); // Damage trigger

        playerAnimator.runtimeAnimatorController = Weapons[currentWeaponIndex].GetComponent<WeaponStats>().WeaponData.WeaponAnimator; // animator override controller
        playerAnimator.speed = Weapons[currentWeaponIndex].GetComponent<WeaponStats>().WeaponData.AnimatorSpeed;

        if (!weaponIconImage)
        {
            weaponIconImage = GameObject.FindGameObjectWithTag("WeaponIconImage")?.GetComponent<Image>();
        }

        if (weaponIconImage)
        {
            weaponIconImage.sprite = Weapons[currentWeaponIndex].GetComponent<WeaponStats>().WeaponData.WeaponIcon;
        }

        currentWeaponIndex++;
    }
}
