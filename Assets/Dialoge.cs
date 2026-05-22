using UnityEngine;
using System.Collections.Generic;

//Made by Michaëla 22-05-2026
[CreateAssetMenu(menuName = "DialogueLine")]
public class Dialoge : ScriptableObject
{
    public string id;
    [TextArea(4,10)]
    public string text;
}
