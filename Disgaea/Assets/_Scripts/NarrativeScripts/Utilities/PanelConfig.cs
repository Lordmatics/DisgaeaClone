using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelConfig : MonoBehaviour
{
    public bool bCharacterTalking;

    public Image avatarImage;
    public Image textBGPanel;
    public Text characterNameText;
    public Text dialogueText;

    private Color activeColor = new Color(255.0f, 222.0f / 255.0f, 137.0f / 255.0f);
    private Color maskActiveColor = new Color(91.0f / 255.0f, 86.0f / 255.0f, 44.0f / 255.0f);

    //private BuildDialogue textAnimator = new BuildDialogue();

    public void ToggleCharacterMask()
    {
        if(bCharacterTalking)
        {
            avatarImage.color = activeColor;
            textBGPanel.color = activeColor;
        }
        else
        {
            avatarImage.color = maskActiveColor;
        }
    }

    //void AnimateText(Dialogue currentDialogue, float speed = 0.05f)
    //{
    //    dialogueText.text = "";

    //    foreach (char letter in currentDialogue.dialogueText)
    //    {
    //        dialogueText.text += letter;
    //        yield return new WaitForSeconds(speed);
    //    }
    //}

    private Coroutine animateTextCoroutine;

    public void StopAnimatingText()
    {
        if (animateTextCoroutine != null)
        {
            StopCoroutine(animateTextCoroutine);
            animateTextCoroutine = null;
        }
    }

    public void ShowCompleteDialogue(Dialogue currentDialogue)
    {
        dialogueText.text = currentDialogue.dialogueText;
    }


    Action myFunc;
    bool bTextFullyShown = false;

    private void OnEnable()
    {
        myFunc += CallBack;    
    }

    private void OnDisable()
    {
        myFunc -= CallBack;   
    }

    public void Configure(Dialogue currentDialogue, bool bUseDelay = false, float delay = 1.0f)
    {
        bTextFullyShown = false;

        ToggleCharacterMask();

        avatarImage.sprite = MasterManager.atlasManager.LoadSprite(currentDialogue.atlasImageName);
        if(characterNameText != null)
            characterNameText.text = currentDialogue.name;

        if (bCharacterTalking)
        {
            if(bUseDelay)
            {
                StartCoroutine(DelayForTextConstruction(currentDialogue, delay));
            }
            else
            {
                //animateTextCoroutine = StartCoroutine(textAnimator.LocalAnimateText(dialogueText, currentDialogue.dialogueText));
                animateTextCoroutine = StartCoroutine(BuildDialogue.AnimateText_Param(dialogueText, currentDialogue.dialogueText, myFunc));
                //Debug.Log("Text should be animating");
                //Debug.Log(currentDialogue.dialogueText);
            }
        }
        else
        {
            // in a game with 1 shared text box this is not required
            //dialogueText.text = "";
        }
    }

    void CallBack()
    {
        bTextFullyShown = true;
        MasterManager.panelManager.UpdatePanelState();
    }

    IEnumerator DelayForTextConstruction(Dialogue currentDialogue, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (bCharacterTalking)
        {
            animateTextCoroutine = StartCoroutine(BuildDialogue.AnimateText_Param(dialogueText, currentDialogue.dialogueText, myFunc));
            //Debug.Log("Text should be animating");
            //Debug.Log(currentDialogue.dialogueText);

        }
    }

}
