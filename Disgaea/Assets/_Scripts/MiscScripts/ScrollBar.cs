using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Manage the actual scrollbar.
public class ScrollBar : Menu {

    public RectTransform scrollbar;
    public RectTransform content;

    public int arrowVisableIndex; // controls arrows position in the visible section of the list.
    public int maximumVisibleArrowIndex;

    private float scrollbarMaxHeight;

    public override void Awake()
    {
        base.Awake();
        SetUpScrollBarHUD();
    }

    void SetUpScrollBarHUD()
    {
        scrollbarMaxHeight = scrollbar.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        float difference = maximumArrowIndex - maximumVisibleArrowIndex; // ie, if 12 total, 7 shown, 5 hidden, the difference = 5;
        scrollbar.sizeDelta = new Vector2(scrollbar.sizeDelta.x, scrollbarMaxHeight / (difference + 1));
        scrollbar.localPosition = new Vector3(0, scrollbar.sizeDelta.y * (difference / 2f));
    }

    //Move Icon Up.
    //If icon is at the top, move selection up.
    //if selection is at the top, do nothing.
    public override void MoveIconUp()
    {
        print("Icon moved up");
        if (arrowPosIndex == 0) // pointer is at the top of the list. don't move anything.
        {
            return;
        }
        else if(arrowVisableIndex == 0 && arrowPosIndex > 0) // pointer is at the top visually but there is unseen elements above it. move content down and scrollbar up.
        {
            arrowPosIndex--;
            content.localPosition -= new Vector3(0, pointerMoveDist, 0);
            scrollbar.localPosition += new Vector3(0, scrollbar.sizeDelta.y, 0);
        }
        else // pointer is not at the top by definition or visually. move pointer up.
        {
            pointer.localPosition += new Vector3(0, pointerMoveDist, 0);
            arrowVisableIndex--;
            arrowPosIndex--;
        }
    }

    //Move Icon Down.
    //If icon is at the bottom, move selection down.
    //if selection is at the bottom, do nothing.
    public override void MoveIconDown()
    {
        print("Icon moved down");
        if (arrowPosIndex == maximumArrowIndex)  // pointer is at the bottom of the list. don't move anything.
        {
            return;
        }
        else if (arrowVisableIndex == maximumVisibleArrowIndex && arrowPosIndex < maximumArrowIndex) // pointer is at the bottom visually but there is unseen elements below it. move content up and scrollbar down.
        {
            arrowPosIndex++;
            content.localPosition += new Vector3(0, pointerMoveDist, 0);
            scrollbar.localPosition -= new Vector3(0, scrollbar.sizeDelta.y, 0);
        }
        else // pointer is not at the bottom by definition or visually. move pointer down.
        {
            pointer.localPosition -= new Vector3(0, pointerMoveDist, 0);
            arrowVisableIndex++;
            arrowPosIndex++;
        }
    }
}
