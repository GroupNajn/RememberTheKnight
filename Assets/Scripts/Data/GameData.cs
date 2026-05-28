using NUnit.Framework.Internal;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class GameData : MonoBehaviour
{

    [field: SerializeField] public int soulsDonatedSinceLast { get; set; } = 0;

    // Set to 4 since the first card of all families cost 4 to unlock. (aka II in the family since the player starts with I) 
    [field:SerializeField] public int SoulsRemainingToNextUnlockWands  {  get; set; } = 4;
    [field:SerializeField] public int SoulsRemainingToNextUnlockCups  {  get; set; } = 4;
    [field:SerializeField] public int SoulsRemainingToNextUnlockPentacles  {  get; set; } = 4;
    [field:SerializeField] public int SoulsRemainingToNextUnlockSwords  {  get; set; } = 4;



    // (Next card in family) - currentSoulsDonated;

    public static void ResetGameData()
    {
        PlayerPrefs.DeleteAll();
    }

    public static void SetSoulsRemaingToNextUnlock(CardContract contract, int amount)
    {

        switch (contract.CardFamily)
        {
            case CardFamily.Wands:

                break;
            case CardFamily.Cups:

                break;
            case CardFamily.Pentacles:

                break;
            case CardFamily.Swords:

                break;
        }
    }


  
    }