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

    public void BootSequence()
    {
        Debug.Log(string.Format("{0} is booting up", GetType().Name));

        leftPanel = GameObject.Find("LeftCharacterPanel").GetComponent<PanelConfig>();
        rightPanel = GameObject.Find("RightCharacterPanel").GetComponent<PanelConfig>();
        currentEvent = JSONAssembly.RunJSONFactoryForIndex(1);
        InitializePanels();
        //currentState = ManagerState.Completed;

        Debug.Log(string.Format("{0} status = {1}", GetType().Name, currentState));
    }

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
        bLeftCharacterTalking = !bLeftCharacterTalking;

        leftPanel.Configure(currentEvent.dialogues[stepIndex]);
        rightPanel.Configure(currentEvent.dialogues[stepIndex + 1]);

        StartCoroutine(MasterManager.animationManager.EnterConversationAnimation());

        stepIndex++;
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

    // occurs on player input
    void UpdatePanelState()
    {
        if(stepIndex < currentEvent.dialogues.Count)
        {
            ConfigurePanels();

            bLeftCharacterTalking = !bLeftCharacterTalking;
            stepIndex++;
        }
        else
        {
            StartCoroutine(MasterManager.animationManager.ExitConversationAnimation());
        }
    }
}
