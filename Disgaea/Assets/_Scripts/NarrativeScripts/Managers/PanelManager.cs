using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JSONFactory;

public class PanelManager : MonoBehaviour, IManager
{
    public ManagerState currentState { get; private set; }

    private PanelConfig leftPanel;

    private PanelConfig rightPanel;

    private NarrativeEvent currentEvent;

    private bool bLeftCharacterTalking = true;

    private int stepIndex = 0;

    bool bDisableInput = true;

    [HideInInspector]
    public float customWait = 1.5f;

    public void BootSequence()
    {
        Debug.Log(string.Format("{0} is booting up", GetType().Name));

        leftPanel = GameObject.Find("LeftCharacterPanel").GetComponent<PanelConfig>();
        rightPanel = GameObject.Find("RightCharacterPanel").GetComponent<PanelConfig>();
        // Stores reference to conversation
        currentEvent = JSONAssembly.RunJSONFactoryForIndex(2);
        InitializePanels();
        //currentState = ManagerState.Completed;

        Debug.Log(string.Format("{0} status = {1}", GetType().Name, currentState));
    }

    // Used to advance the dialogue
    void SpacePressed()
    {
        UpdatePanelState();
    }

    private void OnEnable()
    {
        InputManager.spacePressed += SpacePressed;
    }

    private void OnDisable()
    {
        InputManager.spacePressed -= SpacePressed;
    }

    private void InitializePanels()
    {
        leftPanel.bCharacterTalking = true;
        rightPanel.bCharacterTalking = false;

        // This assumes left person only has one dialogue entry before next person
        //bLeftCharacterTalking = !bLeftCharacterTalking;


        leftPanel.Configure(currentEvent.dialogues[stepIndex], true, customWait);
        for (int i = 0; i < currentEvent.dialogues.Count; i++)
        {
            if(currentEvent.dialogues[i].atlasImageName != currentEvent.dialogues[stepIndex].atlasImageName)
            {
                // Iterate over the conversation and assign the second panel
                // to an image that isnt the first users panel
                rightPanel.Configure(currentEvent.dialogues[i]);
                break;

            }
        }
        // Might wanna add something, to initialise right panel to "placeholder" in the
        // Event the conversation is simply one person only

        //rightPanel.Configure(currentEvent.dialogues[stepIndex + 1]);

        // Animation to begin the conversation
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

    private void ConfigurePanels()
    {
        if(bLeftCharacterTalking)
        {
            leftPanel.bCharacterTalking = true;
            rightPanel.bCharacterTalking = false;

            leftPanel.Configure(currentEvent.dialogues[stepIndex]);
            rightPanel.ToggleCharacterMask();
        }
        else
        {
            leftPanel.bCharacterTalking = false;
            rightPanel.bCharacterTalking = true;

            leftPanel.ToggleCharacterMask();
            rightPanel.Configure(currentEvent.dialogues[stepIndex]);
        }
    }

    int swapSpeakerIndex = 0;
    [SerializeField]
    bool bCanProceed = false;
    // occurs on player input - Space pressed
    public void UpdatePanelState()
    {
        // There is a bug, with the animate text if press space before its finished

        if (bDisableInput) return;
        leftPanel.StopAnimatingText();
        rightPanel.StopAnimatingText();

        // As long as the current index in dialogue
        // Is within bounds
        if(stepIndex < currentEvent.dialogues.Count)
        {
            //if(!bCanProceed)
            //{
                // Iterate over the conversation
                // Starting at the point in the conversation
                // That the game is at
                for (int i = stepIndex; i < currentEvent.dialogues.Count; i++)
                {
                    // At the earliest point from the current point
                    // When a new speaker is found
                    if (currentEvent.dialogues[i].atlasImageName != currentEvent.dialogues[stepIndex].atlasImageName)
                    {
                        swapSpeakerIndex = i - 1;
                        break;
                    }
                }

                // Reveal whole sentence

                if (bLeftCharacterTalking)
                {
                    leftPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
                }
                else
                {
                    rightPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
                }
            //}


            if (bCanProceed)
            {
                bCanProceed = false;
                //bCanProceed = false;
                //bCanProceed = false;
                // STARTS next dialogue
                ConfigurePanels();


                // if the user is a different user at this index, swap
                if (swapSpeakerIndex == stepIndex)
                {
                    bLeftCharacterTalking = !bLeftCharacterTalking;
                }
                stepIndex++;
                return;

            }
            bCanProceed = true;

        }
        else
        {
            if (stepIndex > currentEvent.dialogues.Count)
            {
                // Animation to end the conversation
                StartCoroutine(MasterManager.animationManager.ExitConversationAnimation());
            }

            if (bLeftCharacterTalking)
            {
                if(stepIndex - 1 < currentEvent.dialogues.Count)
                    leftPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
            }
            else
            {
                if (stepIndex - 1 < currentEvent.dialogues.Count)
                    rightPanel.ShowCompleteDialogue(currentEvent.dialogues[stepIndex - 1]);
            }
            stepIndex++;

        }
    }
}
