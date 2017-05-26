using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    public PlayerIcon playerIcon;
    public float moveTime;

    Vector3 defaultPosition = new Vector3(-5, 12, -5);
    Vector3 defaultRotation = new Vector3(59.491f, 45, 0);
    public Vector3 trackingDifference;
    Vector3 reference;
    public int rotationState = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            RotateCamera(true);
        if (Input.GetKeyDown(KeyCode.E))
            RotateCamera(false);
    }

    void LateUpdate ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerIcon.transform.position + trackingDifference, ref reference, moveTime);
	}

    public void LookAtPlayer()
    {
        transform.LookAt(playerIcon.transform);
    }

    public void RotateCamera(bool left)
    {
        float direction = 0;

        if (left)
        {
            direction = -1f;
            if (rotationState == 0 || rotationState == 3)
                direction *= -1f;
            rotationState = ChangeRotationState(--rotationState);
        }
        else
        {
            direction = 1f;
            if (rotationState == 2 || rotationState == 3)
                direction *= -1f;
            rotationState = ChangeRotationState(++rotationState);
        }
        Debug.Log(GetDirectionVector(left) * direction * (Mathf.Abs(defaultPosition.x) * 2));
        Vector3 diff = GetDirectionVector(left) * direction * (Mathf.Abs(defaultPosition.x) * 2);
        transform.position += diff;
        trackingDifference += diff;
        LookAtPlayer();
    }

    Vector3 GetDirectionVector(bool left)
    {
        Debug.Log("Rot state" + rotationState);
        if(rotationState == 0 || rotationState == 2)
        {
            Debug.Log("right");
            if(left)
                return Vector3.forward;
            else
                return Vector3.right;
        }
        else
        {
            Debug.Log("forward");
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
        transform.position = defaultPosition;
        LookAtPlayer();
    }
}
