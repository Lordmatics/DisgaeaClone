using JSONFactory;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/PlayerScripts/NonPlayerScripts/NPC")]
[RequireComponent(typeof(CapsuleCollider))]
public class NPC : MonoBehaviour, IInteractable
{
    public bool bIsInteracting { get; set; }

    Action InteractDelegate;

    public string conversationJSONref = "Shop_Weapon";

    // 2 optinos here
    // Change to scriptable object for each guy
    // use JSON look up for player details
    // Must be dynamic since created characters need to store data in game
    private NarrativeEvent currentEvent;

    private HoverPanelConfig hoverPanel;

    // Not sure about this atm...
    // Basically need a condition for whether the "preview panel" comes back after dialogue 
    [SerializeField][Header("Tick this, if interacting with this person will change scene")]
    private bool bSpecialCase = false;

    private void Start()
    {
        hoverPanel = GameObject.FindObjectOfType<HoverPanelConfig>();
        currentEvent = JSONAssembly.RunJSONFactoryForIndex(conversationJSONref);
        HidePreview();

    }
    private void OnEnable()
    {
        InteractDelegate += SetInteractingFalse;
        InteractDelegate += ShowPreview;
    }

    private void OnDisable()
    {
        InteractDelegate -= SetInteractingFalse;
        InteractDelegate -= ShowPreview;
    }

    public void OnInteract()
    {
        // Open Shop or something

        //Debug.Log("Tried to interact : NPC");

        if (!bIsInteracting)
        {
            // Hide preview panel + delegate it to come back once conversation is over
            HidePreview();
            // Start Conversation
            MasterManager.panelManager.BeginConversationLoadAt(conversationJSONref, InteractDelegate);
            bIsInteracting = true;
            Debug.Log("OnInteract Ran from : NPC");

        }

        // Prevent movement
        // virtual function perhaps for different shops
        // Load Shop
    }

    // Bound to delegate so cannot spam the begin conversation function
    void SetInteractingFalse()
    {
        bIsInteracting = false;
    }

    public void ShowPreview()
    {
        if(currentEvent != null && !bSpecialCase)
        {
            Sprite image = MasterManager.atlasManager.LoadSprite(currentEvent.dialogues[0].atlasImageName);
            if(hoverPanel != null)
                hoverPanel.Configure(image, currentEvent.dialogues[0].name, true);
//            // Spokesperson Icon
//            avatarImage.sprite = MasterManager.atlasManager.LoadSprite(currentEvent.dialogues[0].atlasImageName;
//);

//            // Spokesperson NAme
//            if (characterNameText != null)
//                characterNameText.text = currentDialogue.name;
//            currentEvent.dialogues[0].name;
        }
    }

    public void HidePreview()
    {
        if (currentEvent != null)
        {
            Sprite image = MasterManager.atlasManager.LoadSprite("EmptyIcon");

            hoverPanel.Configure(image, "", false);
        }
    }
}
