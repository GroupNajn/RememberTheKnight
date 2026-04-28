using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponSelect : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] public List<GameObject> Weapons;
   
    int currentWeaponIndex = 0;
    PlayerWeaponManager playerWeaponManager;
    void Start()
    {
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        SelectWeapon();
    }

    public void OnCycle()
    {
       // SelectWeapon();
    }
    public void SelectWeapon()
    {
        currentWeaponIndex = currentWeaponIndex % Weapons.Count; // Wrap around the index
       
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

        currentWeaponIndex++;
    }
}
