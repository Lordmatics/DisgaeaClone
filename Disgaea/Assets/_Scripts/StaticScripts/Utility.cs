using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class Utility
{
    public static Transform ShootRaycastTransform(Vector3 origin, Vector3 direction, float rayLength, LayerMask layermask) // all purpose single raycast method that either returns null or returns the hit object;
    {
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, rayLength, layermask);
        Debug.DrawRay(origin, direction * rayLength, Color.red, 5);
        if (hit.transform != null)
        {
            return hit.transform;
        }
        else
        {
            return null;
        }
    }

    public static Vector3 ShootRaycastVector(Vector3 origin, Vector3 direction, float rayLength, LayerMask layermask) // all purpose single raycast method that either returns a Vector3.zero or returns the hit point world space vector;
    {
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, rayLength, layermask);
        //Debug.DrawRay(origin, direction * rayLength, Color.red, 5);
        if (hit.transform != null)
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }

    // Fischer Shuffle I think this one is called, may be useful
    public static T[] ShuffleArray<T>(T[] array, int seed)
    {
        // Seed ensures an exact shuffle based on int
        System.Random pseudoRandomNumGen = new System.Random(seed);
        for (int i = 0; i < array.Length - 1; i++)
        {
            int randIndex = pseudoRandomNumGen.Next(i, array.Length);
            T tempItem = array[randIndex];
            array[randIndex] = array[i];
            array[i] = tempItem;
        }
        return array;
    }

    public static float RoundNumToDP(float num, int decimalPlaces)
    {
        return Mathf.Round(num * Mathf.Pow(10, decimalPlaces));
    }

    public static int ClampCycleInt(int value, int min, int max)
    {
        if (value > max)
            return min;
        else if (value < min)
            return max;
        return value;
    }

    public static float ClampCycleFloat(float value, float min, float max)
    {
        if (value > max)
            return min;
        else if (value < min)
            return max;
        return value;
    }

    public static bool Approximately(float x, float y, float validDifference)
    {
        if(x <= y + validDifference && 
           x >= y - validDifference ||
           y <= x + validDifference &&
           y >= x - validDifference)
        {
            return true;
        }
        return false;
    }

    public static bool Approximately(float x, float y)
    {
        if (x <= y + 0.05f &&
            x >= y - 0.05f ||
            y <= x + 0.05f &&
            y >= x - 0.05f)
        {
            return true;
        }
        return false;
    }

    /*
    #region INVOKE METHOD
    public static void InvokeMethod(Action method, float timeDelay)
    {
        StartCoroutine(_InvokeMethod(method, timeDelay));
    }

    public static void InvokeMethod(Action[] method, float[] timeDelay)
    {

    }

    public static void InvokeMethod<T>(Action<T> method, float timeDelay, T methodParam)
    {

    }

    public static void InvokeMethod<T>(Action<T>[] method, float[] timeDelay, T[] methodParam)
    {

    }

    static IEnumerator _InvokeMethod(Action method, float timeDelay)
    {
        yield return new WaitForSeconds(timeDelay);
        method();
    }

    static IEnumerator _InvokeMethod(Action[] method, float[] timeDelay)
    {
        for (int i = 0; i < method.Length; i++)
        {
            yield return new WaitForSeconds(timeDelay[i]);
            method[i]();
        }
    }

    static IEnumerator _InvokeMethod<T>(Action<T> method, float timeDelay, T methodParam)
    {
        yield return new WaitForSeconds(timeDelay);
        method(methodParam);
    }

    static IEnumerator _InvokeMethod<T>(Action<T>[] method, float[] timeDelay, T[] methodParam)
    {
        for (int i = 0; i < method.Length; i++)
        {
            yield return new WaitForSeconds(timeDelay[i]);
            method[i](methodParam[i]);
        }
    }
    #endregion
    */
}
