using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BuildDialogue))]
public class BuildDialogueEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Dialogue Construction Text", MessageType.Info);

        base.OnInspectorGUI();
    }
}
