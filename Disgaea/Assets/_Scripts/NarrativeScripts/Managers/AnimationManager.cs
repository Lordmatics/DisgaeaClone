using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;

public class AnimationManager : MonoBehaviour, IManager
{
    public ManagerState currentState { get; private set; }

    Animator panelAnimator;

    public void BootSequence()
    {
       // Debug.Log(string.Format("{0} is booting up", GetType().Name));

        panelAnimator = GameObject.Find("ConversationCanvas").GetComponent<Animator>();
        currentState = ManagerState.Completed;

       // Debug.Log(string.Format("{0} status = {1}", GetType().Name, currentState));
    }

    // Move panels up into screen space
    public IEnumerator EnterConversationAnimation()
    {
        AnimationTuple enterConversationAnim = Constants.AnimationTuples.introAnimation;
        panelAnimator.SetBool(enterConversationAnim.parameter, enterConversationAnim.value);
        yield return new WaitForSeconds(1.0f);
    }

    // Move panels down out of screen space
    // +
    // callback a passed in function to activate once the panels are offscreen
    public IEnumerator ExitConversationAnimation(Action myFunction = null)
    {
        AnimationTuple exitConversationAnim = Constants.AnimationTuples.exitAnimation;
        panelAnimator.SetBool(exitConversationAnim.parameter, exitConversationAnim.value);
        yield return new WaitForSeconds(1.0f);

        if(myFunction != null)
        {
            myFunction();
            myFunction = null;
        }

    }
}
