using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Family Data")]
public class FamilyData : ScriptableObject
{

    [Header("Family")]
    [SerializeField] public CardFamily cardFamily;


}
