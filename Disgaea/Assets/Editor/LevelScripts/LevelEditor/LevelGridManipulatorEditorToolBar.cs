using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class LevelGridManipulatorEditorToolBar : Editor {
    //This is a public variable that gets or sets which of our custom tools we are currently using
    //0 - No tool selected
    //1 - The block eraser tool is selected
    //2 - The "Add block" tool is selected
    public static int SelectedTool
    {
        get
        {
            return EditorPrefs.GetInt("SelectedEditorTool", 0);
        }
        set
        {
            if (value == SelectedTool)
            {
                return;
            }

            EditorPrefs.SetInt("SelectedEditorTool", value);

            switch (value)
            {
                case 0:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                    Tools.hidden = false;
                    break;
                case 1:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                case 2:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", true);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.yellow.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.yellow.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.yellow.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default:
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.black.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.black.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.black.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
            }
        }
    }

    static LevelGridManipulatorEditorToolBar()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;

        //EditorApplication.hierarchyWindowChanged is a good way to tell if the user has loaded a new scene in the editor
        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
        EditorApplication.hierarchyWindowChanged += OnSceneChanged;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;

        EditorApplication.hierarchyWindowChanged -= OnSceneChanged;
    }

    static void OnSceneChanged()
    {
        if (IsInCorrectLevel() == true)
        {
            Tools.hidden = LevelGridManipulatorEditorToolBar.SelectedTool != 0;
        }
        else
        {
            //If the scene has changed and we are in as scene that no longer draws our custom tools menu
            //we want to make sure that the Unity tool Handles are being shown again
            Tools.hidden = false;
        }
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (IsInCorrectLevel() == false)
        {
            return;
        }

        DrawToolsMenu(sceneView.position);
    }

    //I will use this type of function in many different classes. Basically this is useful to 
    //be able to draw different types of the editor only when you are in the correct scene so we
    //can have an easy to follow progression of the editor while hoping between the different scenes
    static bool IsInCorrectLevel()
    {
        return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "DaneScene";
    }

    static void DrawToolsMenu(Rect position)
    {
        //By using Handles.BeginGUI() we can start drawing regular GUI elements into the SceneView
        Handles.BeginGUI();

        //Here we draw a toolbar at the bottom edge of the SceneView
        GUILayout.BeginArea(new Rect(0, position.height - 35, position.width, 20), EditorStyles.toolbarButton);
        {
            string[] buttonLabels = new string[] { "None", "Erase", "Paint", "Adjust Height" };

            GUILayout.BeginHorizontal();

            SelectedTool = GUILayout.SelectionGrid(
                SelectedTool,
                buttonLabels,
                4,
                EditorStyles.toolbarButton,
                GUILayout.Width(400),
                GUILayout.Height(20));
            if(LevelGridManipulationEditorHandle.currentTile != null)
            {
                GUILayout.FlexibleSpace(); // just to get the label on the right, and flexible to make it so if i decide to make more buttons on the toolbar, it stays where it needs to.
                GUILayout.Label("Tile Height: " + LevelGridManipulationEditorHandle.currentTile.heightModifier + ".", GUILayout.Width(100));
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.Label("Tile Height: x", GUILayout.Width(100));
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndArea();

        Handles.EndGUI();
    }
}
