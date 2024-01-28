using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Curtain))]
public class CurtainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Curtain curtain = target as Curtain;
        if (GUILayout.Button("Open"))
        {
            curtain.Open();
        }
        if (GUILayout.Button("Close"))
        {
            curtain.Close();
        }
        DrawDefaultInspector();
    }
}
