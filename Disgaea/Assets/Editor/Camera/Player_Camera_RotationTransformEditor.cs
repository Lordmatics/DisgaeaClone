using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Player_Camera_RotationTransform))]
public class Player_Camera_RotationTransformEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script simply is used to match the rotation of the camera," +
            " in order for legit player movement", MessageType.Info);

        DrawDefaultInspector();
    }
}
