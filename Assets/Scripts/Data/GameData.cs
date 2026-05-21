using UnityEngine;

public class GameData : MonoBehaviour
{

    [field: SerializeField] public int SoulsDoantedSinceLast { get; set; } = 0;
    // Set to 4 since the first card of all families cost 4 to unlock. (aka II in the family since the player starts with I) 
    [field:SerializeField] public int SoulsRemainingToNextUnlock  {  get; set; } = 4;

}