using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverPanelConfig : MonoBehaviour
{
    public Image avatarImage;                   // This panels ref for its avatar image
    public Image textBGPanel;                   // This panels ref for the "shared" dialogue box - Can be changed for other games

    public Text characterNameText;              // This panels ref to its avatars name

    public void Configure(Sprite icon, string name, bool bVisibility)
    {
        avatarImage.sprite = icon;
        textBGPanel.enabled = bVisibility;
        characterNameText.text = name;
    }
}

