using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [SerializeField] private WeaponData weaponData;
    public WeaponData WeaponData => weaponData;

    [SerializeField] GameObject cupsVFX;
    [SerializeField] GameObject wandsVFX;
    [SerializeField] GameObject swordVFX;
    [SerializeField] GameObject pentaclesVFX;

    [SerializeField] string cardName;

    public void EnableWeaponVFX()
    {
        switch (cardName)
        {
            case "King_Cups":
                cupsVFX.SetActive(true);
                break;

            case "King_Wands":
                wandsVFX.SetActive(true);
                break;

            case "King_Swords":
                swordVFX.SetActive(true);
                break;

            case "King_Pentacles":
                pentaclesVFX.SetActive(true);
                break;

            default:
                break;

        }
    }

    public void DisableWeaponVFX()
    {
        if(cupsVFX == null)
        {
            Debug.Log($"{gameObject.name}");
        }

        cupsVFX.SetActive(false);
        wandsVFX.SetActive(false);
        swordVFX.SetActive(false);
        pentaclesVFX.SetActive(false);
    }

    public void SetCardName(string name)
    {
        cardName = name;
    }

}
