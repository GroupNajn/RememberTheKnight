using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Entry")]
public class DialogueEntry : ScriptableObject
{
    public string id;

    [TextArea (10,30)]
    public string fullText;
}
