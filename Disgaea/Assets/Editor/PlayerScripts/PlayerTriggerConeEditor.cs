using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerTriggerCone))]
public class PlayerTriggerConeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("This script handles the collision / trigger events for the player", MessageType.Info);

        base.OnInspectorGUI();
    }
}
