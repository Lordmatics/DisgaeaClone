using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerIcon))]
public class PlayerIconEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Player Icon Script", MessageType.Info);

        base.OnInspectorGUI();
    }
}
