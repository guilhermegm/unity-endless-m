using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_Tab : MonoBehaviour, IPointerClickHandler
{
    public UI_Shop uiShop;
    public Image image;
    public TextMeshProUGUI textBottomLeft;

    public void Init(UI_Shop uiShop, Sprite sprite, string textBottomLeft)
    {
        this.uiShop = uiShop;
        image.sprite = sprite;
        this.textBottomLeft.SetText(textBottomLeft);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiShop.OnTabSelected(this);
    }
}
