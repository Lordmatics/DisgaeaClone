using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerCone : MonoBehaviour
{

    [SerializeField]
    IInteractable currentTarget;

    private void OnTriggerEnter(Collider other)
    {
        currentTarget = other.GetComponent<IInteractable>();
    }

    private void OnTriggerStay(Collider other)
    {
        currentTarget = other.GetComponent<IInteractable>();
    }

    private void OnTriggerExit(Collider other)
    {
        // Make sure this only fires when you leave the right trigger
        if(other.GetComponent<IInteractable>() != null)
            currentTarget = null;
    }

    void FPressed()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(currentTarget != null)
            {
                currentTarget.OnInteract();
            }
        }
    }

    private void OnEnable()
    {
        InputManager.fPressed += FPressed;
        InputManager.qPressed += QPressed;
        InputManager.ePressed += EPressed;
    }

    private void OnDisable()
    {
        InputManager.fPressed -= FPressed;
        InputManager.qPressed -= QPressed;
        InputManager.ePressed -= EPressed;
    }

    int currentRotationValue;
    Coroutine moveCoroutine; // used for conditioning to find the right time to stop and start it.
    Vector3 direction;
    // these are pretty ugh and longwinded. but it all works fine.
    bool wPressed;
    bool aPressed;
    bool sPressed;
    bool dPressed;

    public void QPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(++currentRotationValue, 0, 3);
        if (moveCoroutine != null)
            direction = GetDirection();
    }

    public void EPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(--currentRotationValue, 0, 3);
        if (moveCoroutine != null)
            direction = GetDirection();
    }

    Vector3 GetDirection()
    {
        int dir = GetDir(); // Grabs an index value for current input.
        if (dir != 9) // basically this is here so that if GetDir() returns 9, no direction is given as it'll default to Vector3.zero.
            dir = GetDirDifference(dir); // takes Cam Rotation into account.
        switch (dir)
        {
            case 0:
                return Vector3.forward;
            case 1:
                return Vector3.left + Vector3.forward;
            case 2:
                return Vector3.left;
            case 3:
                return Vector3.back + Vector3.left;
            case 4:
                return Vector3.back;
            case 5:
                return Vector3.right + Vector3.back;
            case 6:
                return Vector3.right;
            case 7:
                return Vector3.forward + Vector3.right;
            default:
                return Vector3.zero;
        }
    }

    int GetDir() // depending on what keys pressed, returns an int.
    {
        if (wPressed && !aPressed && !sPressed && !dPressed) // W pressed.
            return 0;
        else if (wPressed && aPressed && !sPressed && !dPressed) // W & A Pressed
            return 1;
        else if (!wPressed && aPressed && !sPressed && !dPressed) // A Pressed
            return 2;
        else if (!wPressed && aPressed && sPressed && !dPressed) // A & S Pressed
            return 3;
        else if (!wPressed && !aPressed && sPressed && !dPressed) // S Pressed
            return 4;
        else if (!wPressed && !aPressed && sPressed && dPressed) // S & D Pressed
            return 5;
        else if (!wPressed && !aPressed && !sPressed && dPressed) // D Pressed
            return 6;
        else if (wPressed && !aPressed && !sPressed && dPressed) // D & W Pressed
            return 7;
        else
            return 9; // no input. this can be called i think, but only when 
    }

    int GetDirDifference(int dir)
    {
        int val = dir + (currentRotationValue * 2);
        if (val > 7)
            return val - 8;
        else
            return val;
    }
}
