using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public string CardName;


    [Header("Card Stats")]
    [SerializeField] public float damageModifier = 1f;
    [SerializeField] public float healthModifier = 1f;
    [SerializeField] public float LuckModifier = 1f;
    [SerializeField] public float movementModifier = 1f;



}
