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

    void Start()
    {
        playerWeaponManager = GetComponent<PlayerWeaponManager>();
        playerAnimator = GetComponent<Animator>();
        SelectWeapon();
    }

    public void OnCycle()
    {
       // SelectWeapon();
    }
    public void SelectWeapon()
    {
    }
}
