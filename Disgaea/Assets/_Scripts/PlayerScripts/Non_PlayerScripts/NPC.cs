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

    private void OnEnable()
    {
        InteractDelegate += SetInteractingFalse;
    }

    private void OnDisable()
    {
        InteractDelegate -= SetInteractingFalse;
    }

    public void OnInteract()
    {
        // Open Shop or something

        //Debug.Log("Tried to interact : NPC");

        if (!bIsInteracting)
        {
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
}
