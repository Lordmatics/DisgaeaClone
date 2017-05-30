using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerCone : MonoBehaviour
{

    [SerializeField]
    IInteractable currentTarget;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != gameObject)
        {
            currentTarget = other.GetComponent<IInteractable>();
            if (currentTarget != null)
            {
                Debug.Log("Enter" + other.gameObject.name);
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject != gameObject)
        {
            currentTarget = other.GetComponent<IInteractable>();
            if (currentTarget != null)
            {
                //Debug.Log("Stay" + other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject != gameObject)
        //{
            // Make sure this only fires when you leave the right trigger
            if (other.GetComponent<IInteractable>() != null)
            {
                Debug.Log("Exit" + other.gameObject.name);
                currentTarget = null;
            }
        //}
    }

    void FPressed()
    {
        if(currentTarget != null)
        {
            currentTarget.OnInteract();
            Debug.Log("FPRESSED not null");
        }
        else
        {
            //Debug.Log("FPRESSED null");

        }
    }

    public static bool bPreventMovement = false;

    void PreventMovement()
    {
        bPreventMovement = true;
    }

    void EnableMovement()
    {
        bPreventMovement = false;
    }

    private void OnEnable()
    {
        InputManager.fPressed += FPressed;
        InputManager.qPressed += QPressed;
        InputManager.ePressed += EPressed;
        PanelManager.OnEnterConversation += PreventMovement;
        PanelManager.OnConversationEnd += EnableMovement;
        
    }

    private void OnDisable()
    {
        InputManager.fPressed -= FPressed;
        InputManager.qPressed -= QPressed;
        InputManager.ePressed -= EPressed;
        PanelManager.OnEnterConversation -= PreventMovement;
        PanelManager.OnConversationEnd -= EnableMovement;
    }

    int currentRotationValue;

    public void QPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(--currentRotationValue, 0, 3);
        RotateCone();
    }

    public void EPressed()
    {
        currentRotationValue = Utility.ClampCycleInt(++currentRotationValue, 0, 3);
        RotateCone();
    }

    void RotateCone(int dir)
    {
        //float sign = Mathf.Sign(dir);

        //float offset = sign * 90.0f;
        //Vector3 newRot = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y + offset, transform.eulerAngles.z);
        //transform.Rotate(newRot);
    }

    [SerializeField]
    int forwardRotValue = 135;

    void RotateCone()
    {
        switch(currentRotationValue)
        {
            case 0:
                transform.eulerAngles = new Vector3(0, forwardRotValue, 0);
                return;
            case 1:
                transform.eulerAngles = new Vector3(0, forwardRotValue + 90, 0);
                return;
            case 2:
                transform.eulerAngles = new Vector3(0, forwardRotValue + 180, 0);
                return;
            case 3:
                transform.eulerAngles = new Vector3(0, forwardRotValue - 90, 0);
                return;
            default:
                return;
        }
    }
}
