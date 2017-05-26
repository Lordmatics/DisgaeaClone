using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Camera/RotationTransform")]
public class Player_Camera_RotationTransform : MonoBehaviour
{
    /// <summary>
    /// This script needs attaching to the Player GameObject
    /// </summary>
    /// 

    private void Start()
    {
        transform.SetParent(GameObject.FindObjectOfType<Player>().transform);
        transform.localEulerAngles.SetEulerAngleY(45.0f);
        //transform.localEulerAngles = new Vector3(0.0f, 45.0f, 0.0f);
    }

    void QPressed()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.localEulerAngles = new Vector3
                (
                transform.localEulerAngles.x, 
                transform.localEulerAngles.y - 90.0f, 
                transform.localEulerAngles.z
                );
        }
    }

    void EPressed()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.localEulerAngles = new Vector3
                (
                transform.localEulerAngles.x,
                transform.localEulerAngles.y + 90.0f,
                transform.localEulerAngles.z
                );
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
