using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameData))]
public class GameDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //GameData gameData = (GameData)target;

        if (GUILayout.Button("Reset All GameData"))
        {
            GameData.ResetGameData();

            //EditorUtility.SetDirty(gameData);
        }
    }
}