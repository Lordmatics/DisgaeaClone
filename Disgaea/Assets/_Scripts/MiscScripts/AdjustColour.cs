using UnityEngine;
using System.Collections;

[AddComponentMenu("Scripts/MiscScripts/AdjustColour")]
public class AdjustColour : MonoBehaviour {

    [Range(0f, 1f)]
    public float alpha;
    public float timeToChangeColour;
    public enum StartColour
    {
        Red,
        Yellow,
        Green,
        Cyan,
        Blue,
        Purple,
    }
    public StartColour startColour;
    public MeshRenderer meshRenderer;
    int indexer;
    public bool increasing;
    bool adjustIndex;

    void Awake()
    {
        indexer = (int)startColour;
        adjustIndex = increasing;
        if (indexer % 2 == 0)
            increasing = true;
        else
            increasing = false;
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = GetColour();
        StartCoroutine(ChangeColour(GetColour(), GetColourAdjustRate()));
    }

    void SetUpNewColourTransition()
    {
        indexer = AdjustIndexer();
        increasing = !increasing;
        meshRenderer.material.color = GetColour();
        StartCoroutine(ChangeColour(GetColour(), GetColourAdjustRate()));
    }

    int AdjustIndexer()
    {
        if(adjustIndex)
        {
            indexer++;
            if (indexer > 5)
                indexer = 0;
        }
        else
        {
            indexer--;
            if (indexer < 0)
                indexer = 5;
        }
        return indexer;
    }

    Vector4 GetColourAdjustRate()
    {
        if(adjustIndex == true)
        {
            switch (indexer)
            {
                case 0:
                    return new Vector4(0, 0.01f, 0);
                case 1:
                    return new Vector4(0.01f, 0, 0);
                case 2:
                    return new Vector4(0, 0, 0.01f);
                case 3:
                    return new Vector4(0, 0.01f, 0);
                case 4:
                    return new Vector4(0.01f, 0, 0);
                case 5:
                    return new Vector4(0, 0, 0.01f);
            }
        }
        else
        {
            switch (indexer)
            {
                case 0:
                    return new Vector4(0, 0, 0.01f);
                case 5:
                    return new Vector4(0.01f, 0, 0);
                case 4:
                    return new Vector4(0, 0.01f, 0);
                case 3:
                    return new Vector4(0, 0, 0.01f);
                case 2:
                    return new Vector4(0.01f, 0, 0);
                case 1:
                    return new Vector4(0, 0.01f, 0);
            }
        }
        return new Vector4(0, 0.01f, 0);
    }

    Color GetColour()
    {
        switch (indexer)
        {
            case 0:
                return new Color(1, 0, 0, alpha);
            case 1:
                return new Color(1, 1, 0, alpha);
            case 2:
                return new Color(0, 1, 0, alpha);
            case 3:
                return new Color(0, 1, 1, alpha);
            case 4:
                return new Color(0, 0, 1, alpha);
            case 5:
                return new Color(1, 0, 1, alpha);
        }
        return Color.red;
    }

    IEnumerator ChangeColour(Vector4 initialColour, Vector4 changeRate)
    {
        float timePerOperation = timeToChangeColour / 100f;
        Vector4 colour = initialColour;
        for (int i = 0; i < 100; i++)
        {
            switch(increasing)
            {
                case true:
                    colour += changeRate;
                    break;
                case false:
                    colour -= changeRate;
                    break;
            }
            meshRenderer.material.color = colour;
            yield return new WaitForSeconds(timePerOperation);
        }
        SetUpNewColourTransition();
    }
}
