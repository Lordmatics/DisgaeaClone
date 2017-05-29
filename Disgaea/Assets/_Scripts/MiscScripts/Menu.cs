using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour {

    public RectTransform pointer;

    public float timeIncrementBetweenNodes;

    public int arrowPosIndex;
    public int maximumArrowIndex;

    public float pointerMoveDist;

    private bool upPressed;
    private bool downPressed;
    protected MenuConstants menuConstants;

    public virtual void Awake()
    {
        menuConstants = (MenuConstants)Resources.Load("MenuConstants");
        print(menuConstants.GapBetweenNodes);
    }

    //Move Icon Up.
    //If icon is at the top, move selection up.
    //if selection is at the top, do nothing.
    public virtual void MoveIconUp()
    {
        print("Icon moved up");
        if (arrowPosIndex == 0) // pointer is at the top of the list. don't move anything.
        {
            return;
        }
        else // pointer is not at the top, move pointer up.
        {
            pointer.localPosition += new Vector3(0, pointerMoveDist, 0);
            arrowPosIndex--;
        }
    }

    //Move Icon Down.
    //If icon is at the bottom, move selection down.
    //if selection is at the bottom, do nothing.
    public virtual void MoveIconDown()
    {
        print("Icon moved down");
        if (arrowPosIndex == maximumArrowIndex)  // pointer is at the bottom of the list. don't move anything.
        {
            return;
        }
        else // pointer is not at the bottom, move pointer down.
        {
            pointer.localPosition -= new Vector3(0, pointerMoveDist, 0);
            arrowPosIndex++;
        }
    }

    #region Movement IEnumerators
    IEnumerator MoveUp()
    {
        while (upPressed)
        {
            MoveIconUp();
            yield return new WaitForSeconds(timeIncrementBetweenNodes);
        }
    }

    IEnumerator MoveDown()
    {
        while (downPressed)
        {
            MoveIconDown();
            yield return new WaitForSeconds(timeIncrementBetweenNodes);
        }
    }
    #endregion

    #region Movement Delegate Methods
    public void Up() //Delegated Method. Gets Input.
    {
        if (downPressed == true)
            ReleaseDown();
        upPressed = true;
        StartCoroutine("MoveUp");
    }

    public void Down() //Delegated Method. Gets Input.
    {
        if (upPressed == true)
            ReleaseUp();
        downPressed = true;
        StartCoroutine("MoveDown");
    }

    public void ReleaseUp()
    {
        if (!upPressed) // if it's already false, return null.
            return;
        upPressed = false;
        StopCoroutine("MoveUp");
    }

    public void ReleaseDown()
    {
        if (!downPressed) // if it's already false, return null.
            return;
        downPressed = false;
        StopCoroutine("MoveDown");
    }
    #endregion

    #region Activating Input Delegates
    public virtual void EnableScrollBar()
    {
        pointer.gameObject.SetActive(true);
        //InputManager.upArrow += Up;
        //InputManager.downArrow += Down;
        //InputManager.releaseUpArrow += ReleaseUp;
        //InputManager.releaseDownArrow += ReleaseDown;
    }

    public virtual void DisableScrollBar()
    {
        pointer.gameObject.SetActive(false);
        //InputManager.upArrow -= Up;
        //InputManager.downArrow -= Down;
        //InputManager.releaseUpArrow -= ReleaseUp;
        //InputManager.releaseDownArrow -= ReleaseDown;
    }
    #endregion
}
