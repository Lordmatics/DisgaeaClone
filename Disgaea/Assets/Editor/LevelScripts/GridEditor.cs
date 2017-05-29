using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Grid))]
public class GridEditor : Editor
{
    bool updateGrid;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.HelpBox("Grid Script", MessageType.Info);

        EditorGUILayout.HelpBox("Adjust the Vector 2 - The Grid will dynamically resize", MessageType.Info);

        EditorGUILayout.HelpBox("The larger the outline percent the smaller the grid blocks", MessageType.Info);

        Grid grid = target as Grid;

        DrawDefaultInspector();

        GUILayout.Space(10f);
        grid.gridWorldSize.x = EditorGUILayout.IntSlider("Grid Size", (int)grid.gridWorldSize.x, 0, 99);
        if (grid.gridWorldSize.x % 2 == 0) // if evern, add 1.
            grid.gridWorldSize.x += 1;
        grid.gridWorldSize.z = grid.gridWorldSize.x;

        GUILayout.Space(10f);
        updateGrid = GUILayout.Toggle(updateGrid, "Update Grid Every Frame?");
        if (updateGrid)
        {
            if (grid)
            {
                grid.GenerateVisualGrid();
            }
        }
        else
        {
            if(GUILayout.Button("Create/Adjust Visual Grid"))
            {
                if (grid)
                {
                    grid.GenerateVisualGrid();
                }
            }
            if (GUILayout.Button("Save Grid Data"))
            {
                if (grid)
                {
                    grid.SaveTileMap();
                }
            }
            if (GUILayout.Button("Load Grid Data"))
            {
                if (grid)
                {
                    grid.LoadTileMap();
                }
            }
            if (GUILayout.Button("Clear Grid Data"))
            {
                if (grid)
                {
                    grid.ClearTileMap ();
                }
            }
        }
    }
}
