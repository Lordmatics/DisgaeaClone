using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasePanelMisc : MonoBehaviour {

    SpriteRenderer rend_01;
    ParticleSystem.MainModule particleStartColour_01;
    ParticleSystem.MainModule particleStartColour_02;
    ParticleSystem.MainModule particleStartColour_03;
    public float rotationSpeed;
    public float heightVariation;

    // Use this for initialization
    void Awake ()
    {
        rend_01 = GetComponent<SpriteRenderer>();
        particleStartColour_01 = transform.GetChild(0).GetComponent<ParticleSystem>().main;
        particleStartColour_02 = transform.GetChild(1).GetComponent<ParticleSystem>().main;
        particleStartColour_03 = transform.GetChild(2).GetComponent<ParticleSystem>().main;
    }

    private void Update()
    {
        transform.position += new Vector3(0, (-(heightVariation / 2) + Mathf.PingPong(Time.time, heightVariation)) * Time.deltaTime / 5f, 0);
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    void AdjustSpriteColour(Color colour)
    {
        rend_01.color = colour;
        particleStartColour_01.startColor = colour;
        particleStartColour_02.startColor = colour;
        particleStartColour_03.startColor = colour;
    }

    private void OnEnable()
    {
        PlayerIconCube.adjustColour += AdjustSpriteColour;
    }

    private void OnDisable()
    {
        PlayerIconCube.adjustColour -= AdjustSpriteColour;
    }
}
