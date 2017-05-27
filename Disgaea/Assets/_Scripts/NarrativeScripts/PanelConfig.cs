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
    private Color maskActiveColor = new Color(103.0f / 255.0f, 101.0f / 255.0f, 101.0f / 255.0f);

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


    public void Configure(Dialogue currentDialogue)
    {
        ToggleCharacterMask();

        avatarImage.sprite = MasterManager.atlasManager.LoadSprite(currentDialogue.atlasImageName);
        if(characterNameText != null)
            characterNameText.text = currentDialogue.name;

        if(bCharacterTalking)
        {
            BuildDialogue.AnimateText(dialogueText, currentDialogue.dialogueText);
        }
        else
        {
            dialogueText.text = "";
        }
    }

}
