﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildDialogue : MonoBehaviour
{
    private string str;

    public string fullText;

    [SerializeField]
    char space;

    [SerializeField]
    Text text;

    public float waitingBetweenLetter = 0.25f;
    // Use this for initialization
    void Start()
    {
        //Invoke("StartBuilding", delay);
    }

    public void StartBuilding()
    {
        StartCoroutine(AnimateText(fullText));
    }

    IEnumerator AnimateText(string full)
    {
        int i = 0;
        str = "";
        while (i < full.Length)
        {
            if (full[i] == space)
            {
                i++;
                str += " ";
            }
            str += full[i++];
            text.text = str;
            yield return new WaitForSeconds(waitingBetweenLetter);
        }
    }

    public static IEnumerator AnimateText(Text textInQuestion, string dialogueText, float speed = 0.05f)
    {
        textInQuestion.text = "";
        
        foreach(char letter in dialogueText)
        {
            textInQuestion.text += letter;
            yield return new WaitForSeconds(speed);
        }
    }

    public static IEnumerator AnimateText_Param(Text textInQuestion, string dialogueText, Action functionName, float speed = 0.05f)
    {
        textInQuestion.text = "";

        foreach (char letter in dialogueText)
        {
            textInQuestion.text += letter;
            yield return new WaitForSeconds(speed);
        }

        if(functionName != null)
        {
            functionName();
        }
    }
}
