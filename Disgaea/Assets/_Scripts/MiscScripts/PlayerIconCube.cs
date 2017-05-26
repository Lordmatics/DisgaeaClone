using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIconCube : MonoBehaviour {

    public float heightVariation;
    public float rotationSpeed;

    Vector3 startPos;
    MeshRenderer rend;
    SpriteRenderer playerIconSprite;

    private void Awake()
    {
        startPos = transform.position;
        rend = GetComponent<MeshRenderer>();
        playerIconSprite = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        transform.position += new Vector3(0, (-(heightVariation / 2) + Mathf.PingPong(Time.time, heightVariation)) * Time.deltaTime, 0);
        transform.Rotate(new Vector3(rotationSpeed * Time.deltaTime, 0, rotationSpeed * Time.deltaTime));
        playerIconSprite.color = rend.material.color;
    }
}
