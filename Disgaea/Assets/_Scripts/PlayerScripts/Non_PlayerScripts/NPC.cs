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

        Debug.Log("Tried to interact : NPC");

        if (!bIsInteracting)
        {
            // Start Conversation
            MasterManager.panelManager.BeginConversationLoadAt("Shop_Weapon", InteractDelegate);
            bIsInteracting = true;
            Debug.Log("OnInteract Ran from : NPC");

        }

        // Prevent movement

        // Load Shop
    }

    void SetInteractingFalse()
    {
        bIsInteracting = false;
    }
}
