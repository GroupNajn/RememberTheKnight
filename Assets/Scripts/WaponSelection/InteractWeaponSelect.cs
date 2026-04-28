using UnityEngine;

public class InteractWeaponSelect : MonoBehaviour , IInteractable/*, IInteractableUI*/
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    WeaponSelect weaponSelect;

    //public InteractableUIData GetUIData()
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void HideUI()
    //{
    //    throw new System.NotImplementedException();
    //}
    void Start()
    {
        weaponSelect = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponSelect>();

    }
    public void Interact()
    {
        weaponSelect.SelectWeapon();
    }

    //public void SetLookedAt(bool value)
    //{
    //    throw new System.NotImplementedException();
    //}

    //public void ShowUI()
    //{
    //    throw new System.NotImplementedException();
    //}

   

    // Update is called once per frame
   
}
