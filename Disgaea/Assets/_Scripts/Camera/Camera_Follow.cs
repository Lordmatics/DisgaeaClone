using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour {

    public PlayerIcon playerIcon;
    public float moveTime;

    Vector3 defaultPosition = new Vector3(-5, 12, -5);
    Vector3 defaultRotation = new Vector3(59.491f, 45, 0);
    Vector3 reference;
    public int rotationState = 0;

    void LateUpdate ()
    {
        transform.position = Vector3.SmoothDamp(transform.position, playerIcon.transform.position + defaultPosition, ref reference, moveTime);
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
            rotationState = ChangeRotationState(--rotationState);
        }
        else
        {
            direction = 1f;
            rotationState = ChangeRotationState(++rotationState);
        }
        Debug.Log(GetDirectionVector() * direction * (Mathf.Abs(defaultPosition.x) * 2));
        transform.position += GetDirectionVector() * direction * (Mathf.Abs(defaultPosition.x) * 2);
        LookAtPlayer();
    }

    Vector3 GetDirectionVector()
    {
        Debug.Log("Rot state" + rotationState);
        if(rotationState + 1 % 2 == 0)
        {
            Debug.Log("right");
            return Vector3.right;
        }
        else
        {
            Debug.Log("forward");
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
}
