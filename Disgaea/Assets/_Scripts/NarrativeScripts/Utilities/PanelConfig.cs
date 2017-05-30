using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PanelConfig : MonoBehaviour
{

    public Image avatarImage;                   // This panels ref for its avatar image
    public Image textBGPanel;                   // This panels ref for the "shared" dialogue box - Can be changed for other games
    public Text characterNameText;              // This panels ref to its avatars name
    public Text dialogueText;                   // This panels ref for the "shared" text within the textBGPanel

    private Color activeColor = 
        new Color(255.0f, 222.0f / 255.0f, 137.0f / 255.0f);    // HardCoded Colour values
    private Color maskActiveColor = 
        new Color(91.0f / 255.0f, 86.0f / 255.0f, 44.0f / 255.0f);

    public bool bCharacterTalking;              // Bool to determine if this panel is leading the conversation
    private bool bTextFullyShown = false;       // Bool to determine whether text is animating currently or not

    private Action EndOfTextAnimationCall;      // Delegate for end of animated text
    private Coroutine animateTextCoroutine;     // Reference to animation coroutine - so it can be checked for activity

    // Update panel highlights appropriately
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

    // Validity on whether a text animation was stopped successfully
    public bool StopAnimatingText()
    {
        if (animateTextCoroutine != null)
        {
            StopCoroutine(animateTextCoroutine);
            animateTextCoroutine = null;
            return true;
        }
        return false;
    }

    // Reveal whole sentence
    public void ShowCompleteDialogue(Dialogue currentDialogue)
    {      
        //StopAnimatingText(); - This was added as a temporary hack, until a real solution could be implemented
        dialogueText.text = currentDialogue.dialogueText;
    }

    // Use this to initialize the panel to "nothingness"
    // Overloaded function to sever a specific purpose
    public void Configure(bool bEmpty)
    {
        if(bEmpty)
        {
            bTextFullyShown = false;
            ToggleCharacterMask();
            avatarImage.sprite = MasterManager.atlasManager.LoadSprite("EmptyIcon");
        }
    }

    // @currentDialogue - Extracts the data from the loaded narrative event
    // @bUseDelay - Whether to wait for the intro animation to finish before starting
    // @delay - duration for waiting
    public void Configure(Dialogue currentDialogue, bool bUseDelay = false, float delay = 1.0f)
    {
        // NOW, it is important to note,
        // If we decide to want more features in the narrative event, say a big image of the person talking for cutscenes
        // It will need to be added here, syntaxt is obvious :)
        bTextFullyShown = false;

        // Spokesperson Highlighted Panel
        ToggleCharacterMask();

        // Spokesperson Icon
        avatarImage.sprite = MasterManager.atlasManager.LoadSprite(currentDialogue.atlasImageName);

        // Spokesperson NAme
        if(characterNameText != null)
            characterNameText.text = currentDialogue.name;

        // This check is only relevant if we ever split the narrative into multiple speech boxes
        if (bCharacterTalking)
        {
            // This check is to determine whether to wait for the intro animation to finish
            // before printing text or not
            if(bUseDelay)
            {
                StartCoroutine(DelayForTextConstruction(currentDialogue, delay));
            }
            else
            {
                animateTextCoroutine = StartCoroutine(BuildDialogue.AnimateText_Param
                    (dialogueText, currentDialogue.dialogueText, EndOfTextAnimationCall));
            }
        }
        else
        {
            // in a game with 1 shared text box this is not required
            //dialogueText.text = "";
        }
    }

    // Utility Coroutine, for forcing a delay on the text printing
    IEnumerator DelayForTextConstruction(Dialogue currentDialogue, float delay)
    {
        // Make sure string is empty before trying to animate it
        dialogueText.text = "";
        yield return new WaitForSeconds(delay);

        if (bCharacterTalking)
        {
            animateTextCoroutine = StartCoroutine(BuildDialogue.AnimateText_Param
                (dialogueText, currentDialogue.dialogueText, EndOfTextAnimationCall));
        }
    }

    // Delegate CallBack
    private void OnEnable()
    {
        EndOfTextAnimationCall += CallBack;
    }

    private void OnDisable()
    {
        EndOfTextAnimationCall -= CallBack;
    }

    // Function to run for this specific panel
    // Once the Current Dialogue Text has finished animating

    // @ This callback is called each time the text has finished animating
    // @ End of conversation callback is passed in at conversation Start Time
    // See Panel Manager + JSONFactory
    void CallBack()
    {
        // Toggle a bool
        bTextFullyShown = true;
        // And essentially mimic the behaviour of
        // Player Input for fast forwarding through the
        // Dialogue
        // Such that, on their next input, it will know 
        // Where to continue from
        MasterManager.panelManager.UpdatePanelState();
    }
}
