using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility
{
    public static Transform ShootRaycastTransform(Vector3 origin, Vector3 direction, int rayLength, LayerMask layermask) // all purpose single raycast method that either returns null or returns the hit object;
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

    public static Vector3 ShootRaycastVector(Vector3 origin, Vector3 direction, int rayLength, LayerMask layermask) // all purpose single raycast method that either returns a Vector3.zero or returns the hit point world space vector;
    {
        RaycastHit hit;
        Physics.Raycast(origin, direction, out hit, rayLength, layermask);
        Debug.DrawRay(origin, direction * rayLength, Color.red, 5);
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
}
