using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/PlayerScripts/Player")]
public class Player : MonoBehaviour
{
    private PlayerOverWorldMovement movement;

    private void Start()
    {
        movement = GetComponent<PlayerOverWorldMovement>();
    }

    private void Update()
    {
        //if (movement)
        //{
        //    movement.PlayerMovement();
        //}
    }

    private void FixedUpdate()
    {
        if (movement)
        {
            movement.PlayerMovement();
        }
    }


}
