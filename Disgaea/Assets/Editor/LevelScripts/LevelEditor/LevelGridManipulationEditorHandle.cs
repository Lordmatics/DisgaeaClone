using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class LevelGridManipulationEditorHandle : Editor {

    public static Vector3 CurrentHandlePosition = Vector3.zero;
    public static bool IsMouseInValidArea = false;

    static Vector3 m_OldHandlePosition = Vector3.zero;
    static float objectTransformHeight = 1f;

    static bool IsInCorrectLevel()
    {
        return UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "DaneScene";
    }

    static LevelGridManipulationEditorHandle()
    {
        //The OnSceneGUI delegate is called every time the SceneView is redrawn and allows you
        //to draw GUI elements into the SceneView to create in editor functionality
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (IsInCorrectLevel() == false)
        {
            return;
        }

        bool isLevelEditorEnabled = EditorPrefs.GetBool("IsLevelEditorEnabled", true);

        if (isLevelEditorEnabled == false)
        {
            return;
        }

        UpdateHandlePosition();
        UpdateIsMouseInValidArea(sceneView.position);
        UpdateRepaint();

        DrawCubeDrawPreview();
    }

    static void UpdateIsMouseInValidArea(Rect sceneViewRect)
    {
        //Make sure the cube handle is only drawn when the mouse is within a position that we want
        //In this case we simply hide the cube cursor when the mouse is hovering over custom GUI elements in the lower
        //are of the sceneView which we will create in E07
        bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35 && Event.current.mousePosition.x < 300;
        if (isInValidArea != IsMouseInValidArea)
        {
            IsMouseInValidArea = isInValidArea;
            SceneView.RepaintAll();
        }
    }

    static void UpdateHandlePosition()
    {
        if (Event.current == null) // if the player is adjusting the height of a tile, don't update handle position
        {
            return;
        }

        RaycastHit hit = RaycastEditorTransform();
       /* if (tile != null)
        {
            Vector3 offset = Vector3.zero;

            if (EditorPrefs.GetBool("SelectBlockNextToMousePosition", true) == true)
            {
                //CurrentHandlePosition.y = tile.transform.localScale.y;
            }
            else
            {
                //objectTransformHeight = tile.transform.localScale.y;
               // CurrentHandlePosition.y = tile.transform.position.y;
            }

            //CurrentHandlePosition.x = Mathf.Floor(hit.point.x - hit.normal.x * 0.001f + offset.x);
            //CurrentHandlePosition.y = Mathf.Floor(hit.point.y - hit.normal.y * 0.001f + offset.y);
            //CurrentHandlePosition.z = Mathf.Floor(hit.point.z - hit.normal.z * 0.001f + offset.z);
               // CurrentHandlePosition.x = tile.transform.position.x;
               // CurrentHandlePosition.z = tile.transform.position.z;
            //CurrentHandlePosition += new Vector3(0.5f, 0.5f, 0.5f);
        }*/
    }

    static RaycastHit RaycastEditorTransform()
    {
        Vector2 mousePosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);
        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")) == true)
            return hit;
        else
            return hit;
    }

    static void UpdateRepaint()
    {
        //If the cube handle position has changed, repaint the scene
        if (CurrentHandlePosition != m_OldHandlePosition)
        {
            SceneView.RepaintAll();
            m_OldHandlePosition = CurrentHandlePosition;
        }
    }

    static void DrawCubeDrawPreview()
    {
        if (IsMouseInValidArea == false)
        {
            return;
        }

        Handles.color = new Color(EditorPrefs.GetFloat("CubeHandleColorR", 1f), EditorPrefs.GetFloat("CubeHandleColorG", 1f), EditorPrefs.GetFloat("CubeHandleColorB", 0f));

        DrawHandlesCube(CurrentHandlePosition);
    }

    static void DrawHandlesCube(Vector3 center)
    {
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;
        Vector3 p4;
        Vector3 p5;
        Vector3 p6;
        Vector3 p7;
        Vector3 p8;

        if (EditorPrefs.GetBool("SelectBlockNextToMousePosition", true) == true)
        {
            p1 = center + Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p2 = center + Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p3 = center + Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p4 = center + Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;

            p5 = center - Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p6 = center - Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p7 = center - Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p8 = center - Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;
        }
        else
        {
            p1 = center + Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p2 = center + Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p3 = center + Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p4 = center + Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f + Vector3.forward * 0.5f;

            p5 = center - Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p6 = center - Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p7 = center - Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p8 = center - Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f + Vector3.forward * 0.5f;
        }

        //You can use Handles to draw 3d objects into the SceneView. If defined properly the
        //user can even interact with the handles. For example Unitys move tool is implemented using Handles
        //However here we simply draw a cube that the 3D position the mouse is pointing to
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);
        Handles.DrawLine(p3, p4);
        Handles.DrawLine(p4, p1);

        Handles.DrawLine(p5, p6);
        Handles.DrawLine(p6, p7);
        Handles.DrawLine(p7, p8);
        Handles.DrawLine(p8, p5);

        Handles.DrawLine(p1, p5);
        Handles.DrawLine(p2, p6);
        Handles.DrawLine(p3, p7);
        Handles.DrawLine(p4, p8);
    }

    /*

    public static Vector3 CurrentHandlePosition = Vector3.zero;
    public static bool IsMouseInValidArea = false;
    public static Tile currentTile;

    static Vector3 m_OldHandlePosition = Vector3.zero;
    static float objectTransformHeight = 1f;

    static LevelGridManipulationEditorHandle()
    {
        //The OnSceneGUI delegate is called every time the SceneView is redrawn and allows you
        //to draw GUI elements into the SceneView to create in editor functionality
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
        SceneView.onSceneGUIDelegate += OnSceneGUI;
    }

    static void OnSceneGUI(SceneView sceneView)
    {
        if (IsInCorrectLevel() == false)
        {
            return;
        }

        bool isLevelEditorEnabled = EditorPrefs.GetBool("IsLevelEditorEnabled", true);

        if (isLevelEditorEnabled == false)
        {
            return;
        }

        UpdateHandlePosition();
        UpdateIsMouseInValidArea(sceneView.position);
        UpdateRepaint();

        DrawCubeDrawPreview();
    }

    static void UpdateIsMouseInValidArea(Rect sceneViewRect)
    {
        //Make sure the cube handle is only drawn when the mouse is within a position that we want
        //In this case we simply hide the cube cursor when the mouse is hovering over custom GUI elements in the lower
        //are of the sceneView which we will create in E07
        bool isInValidArea = Event.current.mousePosition.y < sceneViewRect.height - 35;

        if (isInValidArea != IsMouseInValidArea)
        {
            IsMouseInValidArea = isInValidArea;
            SceneView.RepaintAll();
        }
    }

    static void UpdateHandlePosition()
    {
        if (Event.current == null) // if the player is adjusting the height of a tile, don't update handle position
        {
            return;
        }

        Vector2 mousePosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);

        Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")) == true)
        {
            Vector3 offset = Vector3.zero;

            if (EditorPrefs.GetBool("SelectBlockNextToMousePosition", true) == true)
            {
                CurrentHandlePosition.y = hit.transform.localScale.y;
            }
            else
            {
                objectTransformHeight = hit.transform.localScale.y;
                CurrentHandlePosition.y = hit.transform.position.y;
            }

            //CurrentHandlePosition.x = Mathf.Floor(hit.point.x - hit.normal.x * 0.001f + offset.x);
            //CurrentHandlePosition.y = Mathf.Floor(hit.point.y - hit.normal.y * 0.001f + offset.y);
            //CurrentHandlePosition.z = Mathf.Floor(hit.point.z - hit.normal.z * 0.001f + offset.z);
            if(EditTilesOnGrid.isSelectedToDrag == false) // if the player is adjusting the height of a tile, Only Update the Y pos, else adjust x and z.
            {
                CurrentHandlePosition.x = hit.transform.position.x;
                CurrentHandlePosition.z = hit.transform.position.z;
                currentTile = hit.transform.GetComponent<Tile>();
            }


            //CurrentHandlePosition += new Vector3(0.5f, 0.5f, 0.5f);
        }
        else if(EditTilesOnGrid.isSelectedToDrag == false)
        {
            currentTile = null;
        }
    }

    static void UpdateRepaint()
    {
        //If the cube handle position has changed, repaint the scene
        if (CurrentHandlePosition != m_OldHandlePosition)
        {
            SceneView.RepaintAll();
            m_OldHandlePosition = CurrentHandlePosition;
        }
    }

    static void DrawCubeDrawPreview()
    {
        if (IsMouseInValidArea == false)
        {
            return;
        }

        Handles.color = new Color(EditorPrefs.GetFloat("CubeHandleColorR", 1f), EditorPrefs.GetFloat("CubeHandleColorG", 1f), EditorPrefs.GetFloat("CubeHandleColorB", 0f));

        DrawHandlesCube(CurrentHandlePosition);
    }

    static void DrawHandlesCube(Vector3 center)
    {
        Vector3 p1;
        Vector3 p2;
        Vector3 p3;
        Vector3 p4;
        Vector3 p5;
        Vector3 p6;
        Vector3 p7;
        Vector3 p8;
            
        if (EditorPrefs.GetBool("SelectBlockNextToMousePosition", true) == true)
        {
            p1 = center + Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p2 = center + Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p3 = center + Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p4 = center + Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;

            p5 = center - Vector3.up * 0.5f + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p6 = center - Vector3.up * 0.5f + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p7 = center - Vector3.up * 0.5f - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p8 = center - Vector3.up * 0.5f - Vector3.right * 0.5f + Vector3.forward * 0.5f;
        }
        else
        {
            p1 = center + Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p2 = center + Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p3 = center + Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p4 = center + Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f + Vector3.forward * 0.5f;

            p5 = center - Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f + Vector3.forward * 0.5f;
            p6 = center - Vector3.up * (objectTransformHeight / 2f) + Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p7 = center - Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f - Vector3.forward * 0.5f;
            p8 = center - Vector3.up * (objectTransformHeight / 2f) - Vector3.right * 0.5f + Vector3.forward * 0.5f;
        }

        //You can use Handles to draw 3d objects into the SceneView. If defined properly the
        //user can even interact with the handles. For example Unitys move tool is implemented using Handles
        //However here we simply draw a cube that the 3D position the mouse is pointing to
        Handles.DrawLine(p1, p2);
        Handles.DrawLine(p2, p3);
        Handles.DrawLine(p3, p4);
        Handles.DrawLine(p4, p1);

        Handles.DrawLine(p5, p6);
        Handles.DrawLine(p6, p7);
        Handles.DrawLine(p7, p8);
        Handles.DrawLine(p8, p5);

        Handles.DrawLine(p1, p5);
        Handles.DrawLine(p2, p6);
        Handles.DrawLine(p3, p7);
        Handles.DrawLine(p4, p8);
    }

    */
}
