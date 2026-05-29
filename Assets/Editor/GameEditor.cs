using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameDataUpdater))]
[CanEditMultipleObjects]

//Comment out this entire script when you want to build Project. 

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