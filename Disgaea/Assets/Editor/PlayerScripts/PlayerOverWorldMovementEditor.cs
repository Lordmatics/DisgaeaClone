using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerOverWorldMovement))]
public class PlayerOverWorldMovementEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Player Movement Script", MessageType.Info);

        base.OnInspectorGUI();
    }
}
