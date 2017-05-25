using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class ChangeSceneEditor : Editor
{

    [MenuItem("OpenScene/DevScenes/NiallScene")]
    public static void OpenNiallScene()
    {
        OpenScene("NiallScene");
    }

    [MenuItem("OpenScene/DevScenes/DaneScene")]
    public static void OpenDaneScene()
    {
        OpenScene("DaneScene");
    }

    static void OpenScene(string name)
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }
}
