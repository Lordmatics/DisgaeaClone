using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{

    Tile m_Target;
    public override void OnInspectorGUI()
    {
        m_Target = target as Tile;

        EditorGUILayout.HelpBox("This script manipulates indivdual tiles", MessageType.Info);

        //base.OnInspectorGUI();

        if(DrawDefaultInspector())
        {
            if (m_Target)
            {
                m_Target.ApplyChanges();
            }
        }

        if(GUILayout.Button("Apply Changes"))
        {
            if(m_Target)
            {
                m_Target.ApplyChanges();
            }
        }
    }
}
