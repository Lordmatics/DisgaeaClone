using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    public static float worldToGameScaleRatio = 0.2f;

    public float maxHeight = 5.0f;

    [Header("The conversion of units is every 1 in world is 0.1f in game")]
    [Range(10,30)]
    public int heightModifier = 10;

    private void Start()
    {
        
    }

    float RoundTo1DP(float num, int decimalPlaces)
    {
        return Mathf.Round(num * Mathf.Pow(10, decimalPlaces));
    }

    // My algorithm
    // 1 - 0
    // 2 - 0.5
    // 3 - 1
    public void ApplyChanges()
    {
        float offset = worldToGameScaleRatio * heightModifier;
        transform.localScale = new Vector3(1.0f, offset, 1.0f);
        // /2 - 0.5f
        float offsetPosition = (offset / 2.0f) - 0.5f;
        transform.position = new Vector3(transform.position.x, offsetPosition, transform.position.z);
        //transform.localScale.SetScaleY(1.0f - heightModifier);
    }

    public void ManipulateTile()
    {
        ApplyChanges();
    }
}
