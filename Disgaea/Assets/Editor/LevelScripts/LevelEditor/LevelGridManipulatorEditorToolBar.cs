using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class LevelGridManipulatorEditorToolBar : Editor {

    public static int currentAmountSelected = 1;
    public static int Selection // a int for a toolbar to swap between no tools, single selection tools or multiselection tools.
    {
        get
        {
            return EditorPrefs.GetInt("SelectionEditor", 0);
        }
        set
        {
            if (value == Selection)
                return;
            EditorPrefs.SetInt("SelectionEditor", value);
            switch(value)
            {
                case 0:
                    Tools.hidden = false;
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);
                    if (EditTilesOnGrid.tiles.Count > 0)
                        EditTilesOnGrid.ResetSelection();
                    break;
                case 1:
                    Tools.hidden = true;
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    if (EditTilesOnGrid.tiles.Count > 0)
                        EditTilesOnGrid.ResetSelection();
                    break;
                default:
                    Tools.hidden = true;
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    break;
            }
        }
    }

    public static int SelectionSingle
    {
        get
        {
            return EditorPrefs.GetInt("SelectionSingleEditor", 0);
        }
        set
        {
            if (value == SelectionSingle)
                return;
            EditorPrefs.SetInt("SelectionSingleEditor", value);
            switch(value)
            {
                case 0: // Paint
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", true);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.yellow.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.yellow.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.yellow.b);
                    break;
                case 1: // Erase
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);
                    break;
                default: // Adjust Height
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.black.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.black.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.black.b);
                    break;
            }
        }
    }


    public static int SelectionMulti
    {
        get
        {
            return EditorPrefs.GetInt("SelectionMultiEditor", 0);
        }
        set
        {
            if (value == SelectionMulti)
                return;
            EditorPrefs.SetInt("SelectionMultiEditor", value);
            switch (value)
            {
                case 0: // Erase
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);
                    break;
                default: // Adjust Height
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.black.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.black.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.black.b);
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

    static bool IsInCorrectLevel()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DaneScene";
    }

    static void OnSceneChanged()
    {
        if (IsInCorrectLevel() == true)
        {
            Tools.hidden = Selection == 0;
        }
        else
        {
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

    static void DrawToolsMenu(Rect position)
    {
        SceneView.RepaintAll();
        currentAmountSelected = 1;
        string[] selection = new string[] { "None", "Single", "Multi"};
        string[] selectionSingle = new string[] { "Paint", "Erase", "Adjust Height" };
        string[] selectionMulti = new string[] { "Erase", "Adjust Height" };
        Handles.BeginGUI();
        {
            // this is just a scaler for the 2 rows of toolbars that could be drawn.
            // is a specific height depending on if 1 or 2 bars are drawn.
            int heightAboveBottom;
            if (Selection == 0)
                heightAboveBottom = 35;
            else
                heightAboveBottom = 60;
            GUILayout.BeginArea(new Rect(0, position.height - heightAboveBottom, position.width, heightAboveBottom - 15)); // this area is just for the tool bars.
            {
                GUILayout.BeginVertical(); // needs to be vertical, so the 2 toolbars aren't on the same line.
                {
                    Selection = GUILayout.SelectionGrid(Selection, selection, selection.Length, GUILayout.Width(100 * selection.Length), GUILayout.Height(20));
                    GUILayout.BeginHorizontal(); // don't really need this anymore. gunna keep it here though in case i decide to add more stuff alongside the toolbars.
                    {
                        if (Selection == 1) // Single
                        {
                            // Single Selection Tool Bar
                            SelectionSingle = GUILayout.SelectionGrid(SelectionSingle, selectionSingle, selectionSingle.Length, GUILayout.Width(100 * selectionSingle.Length), GUILayout.Height(20));
                        }
                        else if (Selection == 2) // Multi
                        {
                            // Multi Selection Tool Bar
                            SelectionMulti = GUILayout.SelectionGrid(SelectionMulti, selectionMulti, selectionMulti.Length, GUILayout.Width(100 * selectionMulti.Length), GUILayout.Height(20));
                        }
                    }
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
            // this rect scales
            Rect rect = new Rect(10, position.height - heightAboveBottom - ((GUI.skin.label.lineHeight + 4) * currentAmountSelected), 286, ((GUI.skin.label.lineHeight + 4) * currentAmountSelected));
            GUILayout.BeginArea(rect);
            {
                // this is just a background for the text;
                GUILayout.Box("", GUILayout.Width(286), GUILayout.Height(rect.size.y));
            }
            GUILayout.EndArea();
            GUILayout.BeginArea(rect);
            {
                GUILayout.BeginVertical();
                {
                    // loop to draw all the text components for sinlge/multi selection. 
                    for (int x = 0; x < currentAmountSelected; x++)
                    {
                        if(x + 1 == currentAmountSelected)
                        {
                            // bottom layer, draw 2 different text areas for last tile height, and overall height adjusted.
                            GUILayout.BeginHorizontal(GUILayout.Height(GUI.skin.label.lineHeight+ 2));
                            {
                                GUILayout.Space(4);
                                GUILayout.Label("Tile Height: " + (x + 100) + ",");
                                GUILayout.FlexibleSpace();
                                GUILayout.Label("Current height adjusted: -160", GUILayout.Width(180));
                            }
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            //draw a text section for that tiles current height.
                            GUILayout.Label("Tile Height: " + (x + 1), GUILayout.Height(GUI.skin.label.lineHeight + 2));
                        }
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndArea();
        }
        if(Selection != 0) // this section is to draw a help box with controls.
        {
            GUILayout.BeginArea(new Rect(10, 10, 100, 50), EditorStyles.helpBox);
            {
                if (Selection == 1) // Single
                {

                }
                else // Multi
                {

                }
            }
            GUILayout.EndArea();
        }

        GUILayout.BeginArea(new Rect(position.width - 480, position.height - 60, 480, 45));
        {
            GUILayout.Box("", GUILayout.Width(480), GUILayout.Height(45));
        }
        GUILayout.EndArea();
        GUILayout.BeginArea(new Rect(position.width - 480, position.height - 60, 480, 45));
        {
            GUILayout.BeginVertical();
            {
                Grid grid = FindObjectOfType<Grid>();
                grid.gridWorldSize.x = EditorGUILayout.IntSlider("Grid Size", (int)grid.gridWorldSize.x, 0, 99, GUILayout.Height(18));
                if (grid.gridWorldSize.x % 2 == 0) // if evern, add 1.
                    grid.gridWorldSize.x += 1;
                grid.gridWorldSize.z = grid.gridWorldSize.x;
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button("Create/Adjust Visual Grid"))
                    {
                        if (grid)
                        {
                            EditTilesOnGrid.ResetSelection();
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
                            EditTilesOnGrid.ResetSelection();
                            grid.LoadTileMap();
                        }
                    }
                    if (GUILayout.Button("Clear Grid Data"))
                    {
                        if (grid)
                        {
                            EditTilesOnGrid.ResetSelection();
                            grid.ClearTileMap();
                        }
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndArea();
        /*if(EditTilesOnGrid.leftMouseCurrentlyDown == true)
        {
            GUILayout.BeginArea(new Rect(EditTilesOnGrid.mousePositionOnRightClick, EditTilesOnGrid.currentMousePosition), EditorStyles.helpBox);
            GUILayout.EndArea();
        }*/
        Handles.EndGUI();


        /*//By using Handles.BeginGUI() we can start drawing regular GUI elements into the SceneView
        Handles.BeginGUI();

        //Here we draw a toolbar at the bottom edge of the SceneView
        GUILayout.BeginArea(new Rect(0, position.height - 55, position.width, 40));
        {
            string[] buttonLabels = new string[] { "None", "Erase", "Paint", "Adjust Height" };
            string[] buttonLabelsMulti = new string[] { "None", "Erase", "Adjust Height" };
            SelectedToolMultiSelection = GUILayout.SelectionGrid(
                SelectedToolMultiSelection,
                buttonLabelsMulti,
                3,
                EditorStyles.toolbarButton,
                GUILayout.Width(300),
                GUILayout.Height(20));

            GUILayout.BeginHorizontal();

            SelectedToolSingleSelection = GUILayout.SelectionGrid(
                SelectedToolSingleSelection,
                buttonLabels,
                4,
                EditorStyles.toolbarButton,
                GUILayout.Width(400),
                GUILayout.Height(20));
            if (LevelGridManipulationEditorHandle.currentTile != null)
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

        Handles.EndGUI();*/
    }

    /*

    //This is a public variable that gets or sets which of our custom tools we are currently using
    //0 - No tool selected
    //1 - The block eraser tool is selected
    //2 - The "Add block" tool is selected
    //3 - the "Adjust Height" tool is selected
    public static int SelectedToolSingleSelection
    {
        get
        {
            return EditorPrefs.GetInt("SelectedEditorTool", 0);
        }
        set
        {
            if (value == SelectedToolSingleSelection)
            {
                return;
            }

            EditorPrefs.SetInt("SelectedEditorTool", value);

            switch (value)
            {
                case 0:// None
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                    Tools.hidden = false;
                    break;
                case 1:// Erase
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                case 2: // Paint
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", true);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.yellow.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.yellow.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.yellow.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default: // Adjust Height
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
    public static int SelectedToolMultiSelection
    {
        get
        {
            return EditorPrefs.GetInt("SelectedEditorMultiTool", 0);
        }
        set
        {
            if (value == SelectedToolMultiSelection)
            {
                return;
            }

            EditorPrefs.SetInt("SelectedEditorMultiTool", value);

            switch (value)
            {
                case 0:// None
                    EditorPrefs.SetBool("IsLevelEditorEnabled", false);

                    Tools.hidden = false;
                    break;
                case 1:// Erase
                    EditorPrefs.SetBool("IsLevelEditorEnabled", true);
                    EditorPrefs.SetBool("SelectBlockNextToMousePosition", false);
                    EditorPrefs.SetFloat("CubeHandleColorR", Color.magenta.r);
                    EditorPrefs.SetFloat("CubeHandleColorG", Color.magenta.g);
                    EditorPrefs.SetFloat("CubeHandleColorB", Color.magenta.b);

                    //Hide Unitys Tool handles (like the move tool) while we draw our own stuff
                    Tools.hidden = true;
                    break;
                default: // Adjust Height
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
            Tools.hidden = LevelGridManipulatorEditorToolBar.SelectedToolSingleSelection != 0;
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
        GUILayout.BeginArea(new Rect(0, position.height - 55, position.width, 40));
        {
            string[] buttonLabels = new string[] { "None", "Erase", "Paint", "Adjust Height" };
            string[] buttonLabelsMulti = new string[] { "None", "Erase", "Adjust Height" };
            SelectedToolMultiSelection = GUILayout.SelectionGrid(
                SelectedToolMultiSelection,
                buttonLabelsMulti,
                3,
                EditorStyles.toolbarButton,
                GUILayout.Width(300),
                GUILayout.Height(20));

            GUILayout.BeginHorizontal();

            SelectedToolSingleSelection = GUILayout.SelectionGrid(
                SelectedToolSingleSelection,
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

    */


}
