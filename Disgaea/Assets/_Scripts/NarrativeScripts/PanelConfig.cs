using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelConfig : MonoBehaviour
{
    public bool bCharacterTalking;

    public Image avatarImage;
    public Image textBGPanel;
    public Text characterNameText;
    public Text dialogueText;

    private Color activeColor = new Color(255.0f, 222.0f / 255.0f, 137.0f / 255.0f);
    private Color maskActiveColor = new Color(91.0f / 255.0f, 86.0f / 255.0f, 44.0f / 255.0f);

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

    public void Configure(Dialogue currentDialogue)
    {
        ToggleCharacterMask();

        avatarImage.sprite = MasterManager.atlasManager.LoadSprite(currentDialogue.atlasImageName);
        if(characterNameText != null)
            characterNameText.text = currentDialogue.name;

        if(bCharacterTalking)
        {
            StartCoroutine(BuildDialogue.AnimateText(dialogueText, currentDialogue.dialogueText));
            Debug.Log("Text should be animating");
            Debug.Log(currentDialogue.dialogueText);

        }
        else
        {
            //dialogueText.text = "";
        }
    }

}
