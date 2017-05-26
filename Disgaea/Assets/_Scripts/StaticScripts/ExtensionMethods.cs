using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{

    public static void ExampleFunction(this GameObject gamObj)
    {
        Debug.Log("Example Function was Ran from: " + gamObj.name);
    }
}
