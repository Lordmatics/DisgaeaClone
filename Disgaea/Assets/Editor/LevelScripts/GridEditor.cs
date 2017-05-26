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

        base.OnInspectorGUI();

        //Grid grid = target as Grid;
        //if(grid)
        //{
        //    grid.CreateGrid();
        //}
    }
}
