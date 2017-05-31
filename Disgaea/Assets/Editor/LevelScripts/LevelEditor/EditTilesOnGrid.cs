using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditTilesOnGrid : Editor
{

    static Transform m_LevelParent;
    public static bool isSelectedToDrag;
    public static Transform LevelParent
    {
        get
        {
            if (m_LevelParent == null)
            {
                GameObject go = GameObject.Find("Level");

                if (go != null)
                {
                    m_LevelParent = go.transform;
                }
            }

            return m_LevelParent;
        }
    }

    static EditTilesOnGrid()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    void OnDestroy()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (IsInCorrectLevel() == false)
        {
            return;
        }

        if (LevelGridManipulatorEditorToolBar.SelectedTool == 0)
        {
            return;
        }

        //By creating a new ControlID here we can grab the mouse input to the SceneView and prevent Unitys default mouse handling from happening
        //FocusType.Passive means this control cannot receive keyboard input since we are only interested in mouse input
        int controlId = GUIUtility.GetControlID(FocusType.Passive);

        //If the left mouse is being clicked and no modifier buttons are being held
        if (Event.current.type == EventType.mouseDown &&
            Event.current.button == 0 &&
            Event.current.alt == false &&
            Event.current.control == false)
        {
            if (LevelGridManipulationEditorHandle.IsMouseInValidArea == true)
            {
                if (LevelGridManipulatorEditorToolBar.SelectedTool == 1)
                {
                    //If there eraser tool is selected, erase the block at the current block handle position
                    RemoveBlock(LevelGridManipulationEditorHandle.CurrentHandlePosition);
                }

                if (LevelGridManipulatorEditorToolBar.SelectedTool == 2)
                {
                    //If the paint tool is selected, create a new block at the current block handle position
                    AddBlock(LevelGridManipulationEditorHandle.CurrentHandlePosition);
                }


                if (LevelGridManipulatorEditorToolBar.SelectedTool == 3)
                {
                    //If the paint tool is selected, create a new block at the current block handle position
                    AdjustHeight(LevelGridManipulationEditorHandle.CurrentHandlePosition);
                    isSelectedToDrag = true;
                    oldMousePosition = Event.current.mousePosition;
                    //Debug.Log("Selected = true");
                }
            }
        }
        if (Event.current.type == EventType.mouseUp &&
            Event.current.button == 0 &&
            Event.current.alt == false &&
            Event.current.control == false)
        {
            if(LevelGridManipulatorEditorToolBar.SelectedTool == 3)
            {
                isSelectedToDrag = false;
                //Debug.Log("Selected = false");
                mouseHeight = 0;
            }
        }

        if (isSelectedToDrag)
        {
            if (Event.current.type == EventType.mouseDrag &&
                Event.current.button == 0 &&
                Event.current.alt == false &&
                Event.current.control == false)
            {
                DraggingTile(sceneView);
                //Debug.Log("Is Dragging");
            }
        }

        //If we press escape we want to automatically deselect our own painting or erasing tools
        if (Event.current.type == EventType.keyDown &&
        Event.current.keyCode == KeyCode.Escape)
        {
            LevelGridManipulatorEditorToolBar.SelectedTool = 0;
        }

        //Add our controlId as default control so it is being picked instead of Unitys default SceneView behaviour
        HandleUtility.AddDefaultControl(controlId);
        //Debug.Log(oldMousePosition == Event.current.mousePosition);
    }

    //Create a new basic cube at the given position
    public static void AddBlock(Vector3 position)
    {
        //Debug.Log("I am going to Add Block Fuctionality later.");
    }

    //Remove a gameobject that is close to the given position
    public static void RemoveBlock(Vector3 position)
    {
        //Debug.Log("I am going to Remove Block Fuctionality later.");
    }

    //Remove a gameobject that is close to the given position
    public static void AdjustHeight(Vector3 position)
    {
        //Debug.Log("I am going to Adjust Height Fuctionality later.");
    }

    static float heightIncrementStepInSceneDistUnits = 2f;
    static float mouseHeight = 0f;
    static Vector2 oldMousePosition = Vector3.zero;
    public static void DraggingTile(SceneView sceneView)
    {
        if (Event.current.mousePosition != oldMousePosition)
        {
            //Debug.Log("Im in");
            if(Event.current.shift == true)
            {
                mouseHeight += (Event.current.mousePosition.y - oldMousePosition.y) * -0.2f;
            }
            else
            {
                mouseHeight += (Event.current.mousePosition.y - oldMousePosition.y) * -1f;
            }

            //Debug.Log(mouseHeight);
            while(LevelGridManipulationEditorHandle.currentTile.heightModifier < (Mathf.RoundToInt(Tile.maxWorldTileHeight - 0.5f) * 10) && mouseHeight > heightIncrementStepInSceneDistUnits)
            {
                //Debug.Log("I'm in again");
                mouseHeight -= heightIncrementStepInSceneDistUnits;
                LevelGridManipulationEditorHandle.currentTile.heightModifier += 1;
                LevelGridManipulationEditorHandle.currentTile.ApplyChanges();
            }
            while (LevelGridManipulationEditorHandle.currentTile.heightModifier > 1 && mouseHeight < -heightIncrementStepInSceneDistUnits)
            {
                //Debug.Log("I'm in this again");
                mouseHeight += heightIncrementStepInSceneDistUnits;
                LevelGridManipulationEditorHandle.currentTile.heightModifier -= 1;
                LevelGridManipulationEditorHandle.currentTile.ApplyChanges();
            }
            if (mouseHeight > heightIncrementStepInSceneDistUnits)
                mouseHeight = heightIncrementStepInSceneDistUnits;
            else if (mouseHeight < -heightIncrementStepInSceneDistUnits)
                mouseHeight = -heightIncrementStepInSceneDistUnits;
            oldMousePosition = Event.current.mousePosition;
        }

        TileDisplay();
    }

    static void TileDisplay()
    {

    }

    //I will use this type of function in many different classes. Basically this is useful to 
    //be able to draw different types of the editor only when you are in the correct scene so we
    //can have an easy to follow progression of the editor while hoping between the different scenes
    static bool IsInCorrectLevel()
    {
        return UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name == "DaneScene";
    }
}
