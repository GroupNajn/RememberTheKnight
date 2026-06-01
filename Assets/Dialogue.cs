using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Represents a single dialogue line used by the dialogue system.
///
/// Each asset contains a unique identifier and the text
/// that should be displayed to the player.
/// </summary>
[CreateAssetMenu(menuName = "DialogueLine")]
public class Dialogue : ScriptableObject
{
   //Made by Michaëla 22-05-2026
    public string id;
    [TextArea(4,10)]
    public string text;
}
