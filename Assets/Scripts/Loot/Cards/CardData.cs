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
    public string cardName;

    public GameObject CardPrefab;
    public Sprite cardImage;
    public Sprite cardInfoImage;

    [Header("Card Material")]
    [SerializeField] public Material frontMaterial;
    [SerializeField] public Material backMaterial;

    [Header("Card Stats")]
    [SerializeField] public float healthModifier = 0f;
    [SerializeField] public float healRegeneraion = 0f;
    [SerializeField] public float staminaModifier = 0f;
    [SerializeField] public float staminaRegeneraion = 0f;

    [SerializeField] public float luckModifier = 0f;
    [SerializeField] public float critChance = 0f;

    [SerializeField] public float actionSpeedModifier = 0f;


    [SerializeField] public float damageModifier = 0f;
    [SerializeField] public float knockbackModifier = 0f;

    [SerializeField] public Vector3 weaponSize = Vector3.zero;
    [SerializeField] public float dodgeSpeedModifier = 0f;

    [Header("Special Card Stats")]
    [SerializeField] public float healModifier = 0f;
    [SerializeField] public bool weaponVFX = false;

    // LIFE STEAL IF TIME

    [Header("Card Prices")]
    [SerializeField] public float cardSoulCost = 0f;

    [Header("Drop Weight")]
    [SerializeField] public float weight = 5f;

    [Header("Card States")]
    [SerializeField] public CardFamily cardFamily;
    [SerializeField] public Tier cardTier;
}
