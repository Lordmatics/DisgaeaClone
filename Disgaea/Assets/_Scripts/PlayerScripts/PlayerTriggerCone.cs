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
        RotateCone();
    }

    public void EPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(--currentRotationValue, 0, 3);
        RotateCone();
    }

    void RotateCone()
    {
        switch(currentRotationValue)
        {
            case 0:
                transform.eulerAngles = new Vector3(0, 0, 0);
                return;
            case 1:
                transform.eulerAngles = new Vector3(0, 270, 0);
                return;
            case 2:
                transform.eulerAngles = new Vector3(0, 180, 0);
                return;
            case 3:
                transform.eulerAngles = new Vector3(0, 90, 0);
                return;
            default:
                return;
        }
    }
}
