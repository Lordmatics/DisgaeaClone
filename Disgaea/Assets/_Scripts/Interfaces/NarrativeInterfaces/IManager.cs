using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://jsonformatter.curiousconcept.com/ // JSON checker for syntax
// https://www.codeandweb.com/blog/2014/03/28/using-spritesheets-with-unity // texture packer pro

public enum ManagerState
{
    Offline, Initializing, Completed
}
public interface IManager
{
    ManagerState currentState { get; }

    void BootSequence();
}


