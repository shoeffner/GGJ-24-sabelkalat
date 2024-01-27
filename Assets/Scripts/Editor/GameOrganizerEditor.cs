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
        DrawDefaultInspector();
    }
}
