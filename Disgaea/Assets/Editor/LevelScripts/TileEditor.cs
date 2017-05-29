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
        DrawDefaultInspector();
        GUILayout.Label("Tile Height: " + m_Target.tileHeight.ToString("000"));
        GUILayout.Label("For every 1 game height unit, tile gets 0.2f higher in worldspace");
        m_Target.heightModifier = EditorGUILayout.IntSlider(m_Target.heightModifier, 1, (Mathf.RoundToInt(Tile.maxWorldTileHeight - 0.5f) * 10));
        if (m_Target)
        {
            m_Target.ApplyChanges();
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
