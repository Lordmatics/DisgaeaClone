using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NPC))]
public class NPC_Editor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script is NPC", MessageType.Info);

        DrawDefaultInspector();
    }
}
