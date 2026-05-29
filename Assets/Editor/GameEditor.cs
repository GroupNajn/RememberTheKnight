using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameDataUpdater))]
[CanEditMultipleObjects]
public class GameDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Reset All GameData"))
        {
            GameDataUpdater.ResetGameData();
        }
    }
}