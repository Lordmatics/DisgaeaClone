using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{

    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Grid Script", MessageType.Info);

        EditorGUILayout.HelpBox("Adjust the Vector 2 - The Grid will dynamically resize", MessageType.Info);

        EditorGUILayout.HelpBox("The larger the outline percent the smaller the grid blocks", MessageType.Info);

        //base.OnInspectorGUI();

        Grid grid = target as Grid;

        if (DrawDefaultInspector())
        {
            if (grid)
            {
                grid.GenerateVisualGrid();
            }
        }

        if(GUILayout.Button("ForceChange"))
        {
            if (grid)
            {
                grid.GenerateVisualGrid();
            }
        }

    }
}
