using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Camera_Follow))]
public class Camera_FollowEditor : Editor {

    Camera_Follow m_Target;
    bool direction;

    //Override OnInspectorGUI() to draw your own editor
    public override void OnInspectorGUI()
    {
        m_Target = (Camera_Follow)target;

        //DrawDefaultInspector tells Unity to draw the inspector like it would if this editor
        //class would not exist. This is usefull if you just want to add some fields to the
        //already existing editor.
        DrawDefaultInspector();
        DrawLookAtButton();
        DrawRotateCameraButton();
    }

    void DrawLookAtButton()
    {
        GUILayout.Space(10);
        if (GUILayout.Button("Look at player."))
        {
            m_Target.LookAtPlayer();
        }
    }

    void DrawRotateCameraButton()
    {
        GUILayout.Space(10);
        direction = GUILayout.Toggle(direction, "Go Left.");
        if (GUILayout.Button("Rotate Camera."))
        {
            m_Target.RotateCamera(direction);
        }
    }
}
