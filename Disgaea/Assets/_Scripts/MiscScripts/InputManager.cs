using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[AddComponentMenu("Scripts/MiscScripts/InputManager")]
public class InputManager : MonoBehaviour
{
    #region Keyboard Input
    #region Pressed
    public static event Action qPressed;
    public static event Action ePressed;

    public static event Action wPressed;
    public static event Action aPressed;
    public static event Action sPressed;
    public static event Action dPressed;

    public static event Action fPressed;

    public static event Action spacePressed;
    public static event Action leftShiftPressed;
    #endregion
    #region Held
    public static event Action qHeld;
    public static event Action eHeld;

    public static event Action wHeld;
    public static event Action aHeld;
    public static event Action sHeld;
    public static event Action dHeld;

    public static event Action fHeld;

    public static event Action spaceHeld;
    #endregion
    #region Released
    public static event Action qReleased;
    public static event Action eReleased;

    public static event Action wReleased;
    public static event Action aReleased;
    public static event Action sReleased;
    public static event Action dReleased;

    public static event Action fReleased;

    public static event Action spaceReleased;
    public static event Action leftShiftReleased;
    #endregion
    #endregion

    #region Mouse Input
    #region Pressed
    #endregion
    #region Held
    #endregion
    #region Released
    #endregion
    #endregion

    #region Joypad Input
    #region Pressed
    #endregion
    #region Held
    #endregion
    #region Released
    #endregion
    #endregion

    void Update()
    {
        KeyBoardInput();
        MouseInput();
        JoypadInput();
    }

    #region UPDATE KEYBOARD INPUT
    void KeyBoardInput()
    {
        KeyBoardPressed();
        KeyBoardHeld();
        KeyBoardReleased();
    }

    void KeyBoardPressed()
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
        if (Input.GetKeyDown(KeyCode.LeftShift))
            if (leftShiftPressed != null)
                leftShiftPressed();
    }

    void KeyBoardHeld()
    {
        if (Input.GetKey(KeyCode.Q))
            if (qHeld != null)
                qHeld();
        if (Input.GetKey(KeyCode.E))
            if (eHeld != null)
                eHeld();
        if (Input.GetKey(KeyCode.W))
            if (wHeld != null)
                wHeld();
        if (Input.GetKey(KeyCode.A))
            if (aHeld != null)
                aHeld();
        if (Input.GetKey(KeyCode.S))
            if (sHeld != null)
                sHeld();
        if (Input.GetKey(KeyCode.D))
            if (dHeld != null)
                dHeld();
        if (Input.GetKey(KeyCode.F))
            if (fHeld != null)
                fHeld();
        if (Input.GetKey(KeyCode.Space))
            if (spaceHeld != null)
                spaceHeld();
    }

    void KeyBoardReleased()
    {
        if (Input.GetKeyUp(KeyCode.Q))
            if (qReleased != null)
                qReleased();
        if (Input.GetKeyUp(KeyCode.E))
            if (eReleased != null)
                eReleased();
        if (Input.GetKeyUp(KeyCode.W))
            if (wReleased != null)
                wReleased();
        if (Input.GetKeyUp(KeyCode.A))
            if (aReleased != null)
                aReleased();
        if (Input.GetKeyUp(KeyCode.S))
            if (sReleased != null)
                sReleased();
        if (Input.GetKeyUp(KeyCode.D))
            if (dReleased != null)
                dReleased();
        if (Input.GetKeyUp(KeyCode.F))
            if (fReleased != null)
                fReleased();
        if (Input.GetKeyUp(KeyCode.Space))
            if (spaceReleased != null)
                spaceReleased();
        if (Input.GetKeyUp(KeyCode.LeftShift))
            if (leftShiftReleased != null)
                leftShiftReleased();
    }
    #endregion

    #region UPDATE MOUSE INPUT
    void MouseInput()
    {

    }
    #endregion

    #region UPDATE GAMEPAD INPUT
    void JoypadInput()
    {

    }
    #endregion
}
