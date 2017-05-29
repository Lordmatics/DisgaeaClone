using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/PlayerScripts/NonPlayerScripts/NPC")]
[RequireComponent(typeof(CapsuleCollider))]
public class NPC : MonoBehaviour, IInteractable
{
    public void OnInteract()
    {
        // Open Shop or something
        Debug.Log("OnInteract Ran from : NPC");

        // Start Conversation
        MasterManager.panelManager.BeginConversationLoadAt("Shop_Weapon");
        // Prevent movement

        // Load Shop
    }
}
