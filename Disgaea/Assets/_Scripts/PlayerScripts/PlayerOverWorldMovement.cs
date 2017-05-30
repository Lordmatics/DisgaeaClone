using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/PlayerScripts/PlayerMovement")]
//[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Rigidbody))]

public class PlayerOverWorldMovement : MonoBehaviour
{
    //private CharacterController controller;
    private Rigidbody controller;

    [SerializeField]
    private Transform rotationTransform;

    [SerializeField] [Range(5.0f,15.0f)]
    private float moveSpeed = 5.0f;

    private void Start()
    {
        controller = GetComponent<Rigidbody>();
    }

    public void PlayerMovement()
    {
        if(PlayerTriggerCone.bPreventMovement)
        {
            return;
        }
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, 0.0f, moveY) * moveSpeed * Time.deltaTime;
        if(rotationTransform != null)
            transform.Translate(movement, rotationTransform);
        //controller.Move(transform.forward * moveSpeed);
    }

}
