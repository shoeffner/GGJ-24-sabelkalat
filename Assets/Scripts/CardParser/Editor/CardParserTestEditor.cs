using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CardParserTest))]
public class CardParserTestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CardParserTest test = target as CardParserTest;
        if (GUILayout.Button("Test"))
        {
            test.Test();
        }
        DrawDefaultInspector();
    }
}
