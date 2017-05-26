using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerCone : MonoBehaviour
{

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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if(currentTarget != null)
            {
                currentTarget.OnInteract();
            }
        }
    }

    private void OnEnable()
    {
        InputManager.qPressed += FPressed;
    }

    private void OnDisable()
    {
        InputManager.qPressed -= FPressed;
    }
}
