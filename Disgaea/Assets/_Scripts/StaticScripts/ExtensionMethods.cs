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

    public static void SetEulerAngleY(this Vector3 eulerAng, float val)
    {
        eulerAng = new Vector3(eulerAng.x, eulerAng.y + val, eulerAng.z);
    }

    public static void ResetToOrigin(this RectTransform trans)
    {
        trans.anchoredPosition = Vector3.zero;
    }
}
