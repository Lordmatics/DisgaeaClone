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

    [MenuItem("OpenScene/DevScenes/OverWorldScene_N")]
    public static void OpenOverWorldScene_N()
    {
        OpenScene("OverWorldScene_N");
    }

    [MenuItem("OpenScene/DevScenes/OverWorldScene_D")]
    public static void OpenOverWorldScene_D()
    {
        OpenScene("OverWorldScene_D");
    }

    static void OpenScene(string name)
    {
        if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene("Assets/Scenes/" + name + ".unity");
        }
    }
}
