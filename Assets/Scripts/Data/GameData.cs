using NUnit.Framework.Internal;
using UnityEngine;

public class GameData : MonoBehaviour
{
    [field: SerializeField] public bool GameCompleted { get; set; } = false;

    [field: SerializeField] public bool FirstTimePlaying { get; set; } = true;

    [field: SerializeField] public int soulsDonatedSinceLastCups { get; set; } = 0;
    [field: SerializeField] public int soulsDonatedSinceLastWands { get; set; } = 0;
    [field: SerializeField] public int soulsDonatedSinceLastPentacles { get; set; } = 0;
    [field: SerializeField] public int soulsDonatedSinceLastSwords { get; set; } = 0;

    // Set to 4 since the first card of all families cost 4 to unlock. (aka II in the family since the player starts with I) 
    [field:SerializeField] public int SoulsRemainingsoulToNextUnlockCups  {  get; set; } = 4;
    [field:SerializeField] public int SoulsRemainingToNextUnlockWands  {  get; set; } = 4;
    [field:SerializeField] public int SoulsRemainingToNextUnlockPentacles  {  get; set; } = 4;
    [field:SerializeField] public int SoulsRemainingToNextUnlockSwords  {  get; set; } = 4;
}
