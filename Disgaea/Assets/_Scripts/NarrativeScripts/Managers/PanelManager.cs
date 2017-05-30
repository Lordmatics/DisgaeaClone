using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;
using System;

public enum MultiLines
{
    Single = 0,
    Multi
}

public class PanelManager : MonoBehaviour, IManager
{
    public ManagerState currentState { get; private set; }

    private PanelConfig leftPanel;          // Ref to Left Panel - Set  in BootSequence
    private PanelConfig rightPanel;         // Ref to Right Panel - Set  in BootSequence

    private NarrativeEvent currentEvent;    // Ref to whole conversation from JSONFactory

    private bool bLeftCharacterTalkingIsNext = true;    // Bool to determine who is next to speak

    private int stepIndex = 0;              // Index to step through the conversation array

    bool bDisableInput = true;              // Bool to prohibit input at certain times - Only Conversation Input

    [HideInInspector]
    public float customWait = 1.5f;         // HardCoded Time to wait, before input is allowed

    public static Action OnConversationEnd;
    // Various possible function that gets passed in when starting a conversation
    // to be called when the conversation ends
    Action StoredOnEnd;

    [SerializeField]
    int swapSpeakerIndex = 0;
    [SerializeField]
    bool bCanProceed = false;

    Coroutine exitCoroutine;

    public delegate void OnConversationStart();
    public static event OnConversationStart OnEnterConversation;

    // occurs on player input - Space pressed

    // NEED TO FORCE A WAIT IN HERE
    // TO LET THE ANIMATE TEXT COROUTINE BEGIN
    // BEFORE TRYING TO SKIP IT
    // SINCE IF U PRESS SPACE BEFORE IT STARTS IT WILL
    // FULLY PRINT, THEN START ANIMATING FROM SCRATCH

    bool bStopLeftAnim;
    bool bStopRightAnim;

    private void OnEnable()
    {
        InputManager.spacePressed += SpacePressed;
        OnConversationEnd += EndOfConversationReset;
    }

    private void OnDisable()
    {
        InputManager.spacePressed -= SpacePressed;
        OnConversationEnd -= EndOfConversationReset;
        OnConversationEnd -= StoredOnEnd;
    }

    public void BootSequence()
    {
        //Debug.Log(string.Format("{0} is booting up", GetType().Name));

        leftPanel = GameObject.Find("LeftCharacterPanel").GetComponent<PanelConfig>();
        rightPanel = GameObject.Find("RightCharacterPanel").GetComponent<PanelConfig>();
    }



    void ResetConversationConditions()
    {
        stepIndex = 0;
        leftPanel.dialogueText.text = "";
        rightPanel.dialogueText.text = "";
        bStopLeftAnim = false;
        bStopRightAnim = false;
        leftPanel.bCharacterTalking = true;
        rightPanel.bCharacterTalking = false;
    }

    // Initiates a conversation by loading it from json with a string
    // The Action parameter is specific to who is calling the function
    // As that passed in function will run once the conversation has finished
    // For example, open up the weapon shop
    public void BeginConversationLoadAt(string conversationStringFromJSONFactory, Action del = null)
    {
        if(OnEnterConversation != null)
        {
            OnEnterConversation();
        }
        // If optional function has been passed in
        if(del != null)
        {
            // Reset the delegate
            OnConversationEnd -= StoredOnEnd;
            // Cache the parameter function
            StoredOnEnd = del;
            // add the cache to our end of conversation delegate
            OnConversationEnd += StoredOnEnd;
        }

        // Make sure the conversation is starting from the beginning
        // With appropriate resets
        ResetConversationConditions();

        // Assign the current conversation with the one loaded from the string parameter
        currentEvent = JSONAssembly.RunJSONFactoryForIndex(conversationStringFromJSONFactory);

        // Configure the Left starting panel, with the appropriate conversation dialogue
        // This first configuration requires a delay
        // To match the animation
        leftPanel.Configure(currentEvent.dialogues[stepIndex], true, customWait);

        // Configure Right Panel - conditions for if there is only one speaker take place here
        for (int i = 0; i < currentEvent.dialogues.Count; i++)
        {
            if(currentEvent.dialogues[i].atlasImageName != currentEvent.dialogues[stepIndex].atlasImageName)
            {
                // Iterate over the conversation and assign the second panel
                // to an image that isnt the first users panel
                rightPanel.Configure(currentEvent.dialogues[i]);
                break;

            }
            // Add code to set default image template it nothing, if its a one man conversation
            rightPanel.Configure(true);
        }

        // Animation to begin the conversation
        // Custom wait simply prohibits input whilst the animation is occuring
        StartCoroutine(MasterManager.animationManager.EnterConversationAnimation());
        StartCoroutine(CustomWait());

        // Index to increment through the dialogue
        stepIndex++;
    }

    private IEnumerator CustomWait()
    {
        yield return new WaitForSeconds(customWait);
        bDisableInput = false;
    }

    // Used to advance the dialogue
    void SpacePressed()
    {
        UpdatePanelState();
    }

    public void UpdatePanelState()
    {
        // If spamming space whilst animation is happenning...
        // Nothing will break
        if (bDisableInput) return;

        // Cache reference to, whether or not the ending of the animation was successful
        // Required to prevent a wierd bug conflicting with show full text
        // And continuation of animation
        bStopLeftAnim = leftPanel.StopAnimatingText();
        bStopRightAnim = rightPanel.StopAnimatingText();

        // As long as the current index in dialogue
        // Is within bounds
        if (stepIndex < currentEvent.dialogues.Count)
        {
            // First attempt that didn't work - can reflect on this
            #region _CODE_
            //if(!bCanProceed)
            //{
            // Iterate over the conversation
            // Starting at the point in the conversation
            // That the game is at
            //int counter = 0;
            //for (int i = stepIndex; i < currentEvent.dialogues.Count; i++)
            //{
            //    // At the earliest point from the current point
            //    // When a new speaker is found

            //// @ step 0 - counter = 0
            //// @ 0 laharl != laharl - false counter++
            //// @ 1 etna != laharl - true - swapindex = 1 - counter = 0

            //// @ step 2 - counter = 0
            //// @ 2 laharl != laharl - false - counter = 1
            //// @ 3 laharl != laharl - false - counter = 2
            //// @ 4 laharl != etna - true - swapindex = 4 - counter = 2

            //    // As soon as the atlasimagename is different to the current name, store the index, when that is
            //    if (currentEvent.dialogues[i].atlasImageName != currentEvent.dialogues[stepIndex].atlasImageName)
            //    {
            //    //Debug.Log(string.Format("Image name at loop: {0} , Image name at step index: {1}", currentEvent.dialogues[i].atlasImageName, currentEvent.dialogues[stepIndex].atlasImageName));
            //        swapSpeakerIndex = i;
            //        break;
            //    }
            //    counter++;
            //}
#endregion
        
            // Reveal whole sentence - Assuming there is no animating text
            // Occuring at this time
            if (bLeftCharacterTalkingIsNext)
            {
                if(bStopLeftAnim || bStopRightAnim)
                    leftPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
            }
            else
            {
                if(bStopRightAnim || bStopLeftAnim)
                    rightPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
            }

            // If the text is fully shown or the animation was cut short, this will be true
            // This section, enables the continuation of the conversation
            if (bCanProceed)
            {
                // set this to false immediately, since once this full text has been shown,
                // we need to prep it to be ready for animating the next line +
                // subsequently wait till bProceed is true again
                bCanProceed = false;

                // Since im implementing a left and right person narrative system
                // It was important to determine whether, one spokesperson
                // had multiple lines of dialgoue...
                // If they simply have one line, then swap to the other spokesperson next
                // If not, continue 
                if (!currentEvent.dialogues[stepIndex - 1].bMultiLines)
                {
                    bLeftCharacterTalkingIsNext = !bLeftCharacterTalkingIsNext;
                    
                }

                // Update Sprite Data + Conversation text + Other misc info from Narrative Event
                ConfigurePanels();

                // Increment index, to proceed with conversation
                stepIndex++;

                // Early exit, since we can move on to next piece of dialogue
                return;

            }          

            // May need to do some more adjustments for other side - Think its good now
            // If there was a succesful stop of text animating - it means the text is fully visible
            // Thus you are ready to proceed
            if(bStopRightAnim || bStopLeftAnim)
                bCanProceed = true;
        }
        else
        {
            // This segment will run, if you are on the last line / last line + 1, of the conversation

            // This chunk relies on the step index to be one above the max number of lines in the conversation
            if (stepIndex > currentEvent.dialogues.Count)
            {
                // Animation to end the conversation
                if(exitCoroutine == null)
                    exitCoroutine = StartCoroutine(MasterManager.animationManager.ExitConversationAnimation(OnConversationEnd));
                return;
            }
            
            // Code to enable the sentence finisher functionality for this final use case specifically
            if (bLeftCharacterTalkingIsNext)
            {
                if(stepIndex - 1 < currentEvent.dialogues.Count)
                    leftPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
            }
            else
            {
                if (stepIndex - 1 < currentEvent.dialogues.Count)
                    rightPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
            }
            // Increment index, to prep for ExitConversation animation
            stepIndex++;
        }
    }

    // Left and Right Panel setup - Sprite + Data
    private void ConfigurePanels()
    {
        // Depending on who is talking next
        // Certain flags need to be set, within the Panel Config classes
        if (bLeftCharacterTalkingIsNext)
        {
            leftPanel.bCharacterTalking = true;
            rightPanel.bCharacterTalking = false;

            // Loads sprite, dialogue text + extendable for anything else from narrative event
            leftPanel.Configure(currentEvent.dialogues[stepIndex]);
            // Swaps the highlight colour for the panel coming up next
            rightPanel.ToggleCharacterMask();
        }
        else
        {
            leftPanel.bCharacterTalking = false;
            rightPanel.bCharacterTalking = true;

            // The toggle is required to keep each other panel flip flopping, based on current speaker
            leftPanel.ToggleCharacterMask();
            rightPanel.Configure(currentEvent.dialogues[stepIndex]);
        }
    }

    // THERE IS A BUG HERE SOMEWHERE -
    // FUNCTION GETS CALLED MULTIPLE TIMES OCCASIONALLY
    // Think this is fixed now
    public void EndOfConversationReset()
    {
        Debug.Log("FunctionCalled TEST");
        exitCoroutine = null;
        stepIndex = 0;
        bLeftCharacterTalkingIsNext = true;
        // May need to set disable input to true here - After testing, this isnt required but logically it should be
        bDisableInput = true;
    }
}
