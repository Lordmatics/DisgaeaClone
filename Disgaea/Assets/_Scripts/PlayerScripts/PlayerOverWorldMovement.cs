using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/PlayerScripts/PlayerMovement")]
[RequireComponent(typeof(CharacterController))]
public class PlayerOverWorldMovement : MonoBehaviour
{
    private CharacterController controller;

    [SerializeField]
    private Transform rotationTransform;

    [SerializeField]
    private float moveSpeed = 0.1f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void PlayerMovement()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0.0f, moveY) * moveSpeed;
        if(rotationTransform != null)
            //transform.Translate(movement, rotationTransform);
        controller.Move(transform.forward * moveSpeed);
    }

}
