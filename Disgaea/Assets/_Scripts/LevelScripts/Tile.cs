using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/LevelScripts/Tile")]
public class Tile : MonoBehaviour
{
    public void SetTile(int height)
    {
        heightModifier = height;
        tileHeight = height;
        ApplyChanges();
    }
    public static float maxWorldTileHeight = 16.5f;
    public static float worldToGameScaleRatio = 0.1f;

    //public float maxHeight = 5.0f;

    //[Header("The conversion of units is every 1 in world is 0.1f in game")]
    //[Range(5,30)]
    [HideInInspector]
    public int heightModifier;
    [HideInInspector]
    public int tileHeight;
    int modifier;

    // My algorithm
    // 1 - 0
    // 2 - 0.5
    // 3 - 1
    public void ApplyChanges()
    {
        modifier = heightModifier + 9; // to make slider start at 1 instead of 10, then height being that 10 - 9. 
        float offset = worldToGameScaleRatio * modifier;
        transform.localScale = new Vector3(1.0f, offset, 1.0f);
        // /2 - 0.5f
        float offsetPosition = (offset / 2.0f) - 0.5f;
        transform.position = new Vector3(transform.position.x, offsetPosition, transform.position.z);
        //transform.localScale.SetScaleY(1.0f - heightModifier);
        tileHeight = modifier - 9;
        float tileSpriteHeight = transform.localScale.y + 0.5f - transform.position.y;
        transform.GetChild(0).position = new Vector3(transform.position.x, (0.1f * modifier) - 0.48f, transform.position.z);
    }

    public int GetHeight()
    {
        return tileHeight;
    }

    public Vector3 GetHeightVector()
    {
        return new Vector3(transform.position.x, (0.1f * modifier) - 0.5f, transform.position.z);
    }
}
