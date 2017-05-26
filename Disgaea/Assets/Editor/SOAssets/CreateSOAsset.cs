using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeInformationDataAsset
{
    [MenuItem("Assets/Create/ScriptableObjects/NodeInformation")]
    public static void CreateAsset()
    {
        ScriptableObjectUtility.CreateAsset<NodeInformation>();
    }
}
