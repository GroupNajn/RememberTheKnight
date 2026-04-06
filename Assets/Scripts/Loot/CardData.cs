using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Card Data")]
public class CardData : ScriptableObject
{
    public string CardName;

    [Header("Card Stats")]
    [SerializeField] public float damageModifier = 0f;
    [SerializeField] public float healthModifier = 0f;
    [SerializeField] public float LuckModifier = 0f;
    [SerializeField] public float movementModifier = 0f;



}
