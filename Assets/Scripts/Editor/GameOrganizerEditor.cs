using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameOrganizer))]
public class GameOrganizerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        GameOrganizer test = target as GameOrganizer;
        if (GUILayout.Button("Add score"))
        {
            test.AddScore(2);
        }
        if (GUILayout.Button("Reduced Score"))
        {
            test.AddScore(-2);
        }
        if (GUILayout.Button("Next Round"))
        {
            test.NextRound();
        }
        if (GUILayout.Button("Game Win"))
        {
            //test.AddScore(1000);
            //test.OnGameWin.Invoke();
            test.DebugPrepareGameWin();
        }
        if (GUILayout.Button("Game Lose"))
        {
            //test.AddScore(-10000);
            //test.OnGameLose();
            test.DebugPrepareGameLose();
        }
        DrawDefaultInspector();
    }
}
