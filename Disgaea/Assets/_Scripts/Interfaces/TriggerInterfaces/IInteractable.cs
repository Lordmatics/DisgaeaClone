using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool bIsInteracting { get; set; }

    void OnInteract();
}
