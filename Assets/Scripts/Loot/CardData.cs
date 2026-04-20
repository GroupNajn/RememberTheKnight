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


    public string CardName;

    public GameObject CardPrefab;
    public Sprite cardImage;

    [Header("Card Stats")]
    [SerializeField] public float damageModifier = 1f;
    [SerializeField] public float healthModifier = 1f;
    [SerializeField] public float LuckModifier = 1f;
    [SerializeField] public float staminaModifier = 1f;
    [SerializeField] public float movementModifier = 1f;



}
