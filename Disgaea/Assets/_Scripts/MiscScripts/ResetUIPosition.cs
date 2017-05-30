using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetUIPosition : MonoBehaviour
{

    private RectTransform pos;

	// Use this for initialization
	void Start ()
    {
        pos = GetComponent<RectTransform>();
        if (pos != null)
            pos.ResetToOrigin();
        else
            throw new System.Exception("No Rect Transform on object: " + gameObject.name);
	}
}
