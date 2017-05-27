using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Scripts/MiscScripts/InputManager")]
public class InputManager : MonoBehaviour
{

    public static event Action qPressed;
    public static event Action ePressed;

    public static event Action wPressed;
    public static event Action aPressed;
    public static event Action sPressed;
    public static event Action dPressed;

    public static event Action fPressed;

    public static event Action spacePressed;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            if (qPressed != null)
                qPressed();
        if (Input.GetKeyDown(KeyCode.E))
            if (ePressed != null)
                ePressed();
        if (Input.GetKeyDown(KeyCode.W))
            if (wPressed != null)
                wPressed();
        if (Input.GetKeyDown(KeyCode.A))
            if (aPressed != null)
                aPressed();
        if (Input.GetKeyDown(KeyCode.S))
            if (sPressed != null)
                sPressed();
        if (Input.GetKeyDown(KeyCode.D))
            if (dPressed != null)
                dPressed();
        if (Input.GetKeyDown(KeyCode.F))
            if (fPressed != null)
                fPressed();

        if (Input.GetKeyDown(KeyCode.Space))
            if (spacePressed != null)
                spacePressed();
    }
}
