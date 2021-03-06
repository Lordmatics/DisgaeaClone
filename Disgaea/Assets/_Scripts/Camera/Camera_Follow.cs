﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Camera/Camera_Follow")]
public class Camera_Follow : MonoBehaviour
{

    public Transform player;
    public float moveTime;

    Transform parent;
    Vector3 defaultPosition = new Vector3(-5, 12, -5);
    Vector3 defaultRotation = new Vector3(59.491f, 45, 0);
    public Vector3 trackingDifference;
    Vector3 reference_01;
    Vector3 reference_02;
    public int rotationState = 0;
    bool camMovingFromRotation;

    private void Awake()
    {
        parent = transform.parent;
    }

    void FixedUpdate ()
    {
        if (parent.position != player.position)
        {
            parent.position = Vector3.SmoothDamp(parent.position, player.position, ref reference_01, moveTime);
            if (Utility.Approximately(parent.position.x, player.position.x) &&
                Utility.Approximately(parent.position.z, player.position.z))
            {
                parent.position = player.position;
            }
        }
        if (trackingDifference != transform.localPosition)
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, trackingDifference, ref reference_02, moveTime);
            LookAtPlayer();
            if (Utility.Approximately(transform.localPosition.x, trackingDifference.x) &&
                Utility.Approximately(transform.localPosition.z, trackingDifference.z))
            {
                transform.localPosition = trackingDifference;
            }
        }
	}

    public void LookAtPlayer()
    {
        transform.LookAt(parent);
    }

    public void RotateCamera(bool left)
    {
        float direction = 0;

        if (left)
        {
            direction = -1f;
            if (rotationState == 0 || rotationState == 3)
                direction *= -1f;
            rotationState = Utility.ClampCycleInt(--rotationState, 0, 3); // ChangeRotationState(--rotationState);
        }
        else
        {
            direction = 1f;
            if (rotationState == 2 || rotationState == 3)
                direction *= -1f;
            rotationState = Utility.ClampCycleInt(++rotationState, 0, 3); // ChangeRotationState(++rotationState);
        }
        Vector3 diff = GetDirectionVector(left) * direction * (Mathf.Abs(defaultPosition.x) * 2);
        trackingDifference += diff;
    }

    Vector3 GetDirectionVector(bool left)
    {
        //Debug.Log("Rot state" + rotationState);
        if(rotationState == 0 || rotationState == 2)
        {
            if(left)
                return Vector3.forward;
            else
                return Vector3.right;
        }
        else
        {
            if (left)
                return Vector3.right;
            else
                return Vector3.forward;
        }
    }

    int ChangeRotationState(int state)
    {
        if(state < 0)
        {
            return 3;
        }
        else if(state > 3)
        {
            return 0;
        }
        return state;
    }

    public void MoveToDefaultPos()
    {
        rotationState = 0;
        transform.localPosition = defaultPosition;
        LookAtPlayer();
        trackingDifference = defaultPosition;
    }

    void QPressed()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RotateCamera(true);
            camMovingFromRotation = true;
        }
    }

    void EPressed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            RotateCamera(false);
            camMovingFromRotation = true;
        }
    }

    private void OnEnable()
    {
        InputManager.qPressed += QPressed;
        InputManager.ePressed += EPressed;
    }

    private void OnDisable()
    {
        InputManager.qPressed -= QPressed;
        InputManager.ePressed -= EPressed;
    }
}
