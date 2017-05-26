using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Tile))]
public class TileEditor : Editor
{

    public override void OnInspectorGUI()
    {

        EditorGUILayout.HelpBox("This script manipulates indivdual tiles", MessageType.Info);

        base.OnInspectorGUI();

        Tile tile = target as Tile;
        if (tile)
        {
            tile.ManipulateTile();
        }

        if(GUILayout.Button("Apply Changes"))
        {
            if(tile)
            {
                tile.ApplyChanges();
            }
        }
    }
}
