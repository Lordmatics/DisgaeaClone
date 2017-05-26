using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static void ExampleFunction(this GameObject gamObj)
    {
        Debug.Log("Example Function was Ran from: " + gamObj.name);
    }

    public static void SetScaleY(this Vector3 scale, float offset)
    {
        scale = new Vector3(scale.x, scale.y + offset, scale.z);
    }
}
