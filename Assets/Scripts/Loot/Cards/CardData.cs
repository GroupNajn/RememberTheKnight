using System.Xml;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    /* 
     *  Cups : Health - General Endurance
     *  Pentacles : Luck 
     *  Swords : Damage 
     *  Wands - Movement 
     * 
     */

    


    public string cardID;

    public GameObject CardPrefab;
    public Sprite cardImage;

    [Header("Card Material")]
    [SerializeField] public Material frontMaterial;
    [SerializeField] public Material backMaterial;

    [Header("Card Stats")]
    [SerializeField] public float healthModifier = 0f;
    [SerializeField] public float staminaModifier = 0f;

    [SerializeField] public float LuckModifier = 0f;
    [SerializeField] public float critChance = 0f;

    [SerializeField] public float walkSpeedModifier = 0f;
    [SerializeField] public float sprintSpeedModifier = 0f;
    [SerializeField] public float dodgeSpeedModifier = 0f;
    [SerializeField] public float damageModifier = 0f;

    [SerializeField] public float healModifier = 0f;
    [SerializeField] public float knockbackModifier = 0f;

    [SerializeField] public Vector3 weaponSize = Vector3.zero;

    [Header("Card Prices")]
    [SerializeField] public float cardSoulCost = 0f;

    [Header("Drop Weight")]
    [SerializeField] public float weight = 5f;

    [Header("Card States")]
    [SerializeField] public CardFamily cardFamily;
    [SerializeField] public Tier cardTier;
}
