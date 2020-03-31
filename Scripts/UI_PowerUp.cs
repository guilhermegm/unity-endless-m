using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_PowerUp : MonoBehaviour, IPointerClickHandler
{
    public UI_Shop uiShop;
    public Image image;
    public TextMeshProUGUI text;
    public TextMeshProUGUI buttonText;

    public void Init(UI_Shop uiShop, Sprite sprite, string text, string buttonText)
    {
        this.uiShop = uiShop;
        image.sprite = sprite;
        this.text.SetText(text);
        this.buttonText.SetText(buttonText);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiShop.OnPowerUpClicked(this);
    }
}
