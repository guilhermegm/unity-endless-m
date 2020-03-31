using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class UI_UseButton : MonoBehaviour, IPointerClickHandler
{
    public UI_Shop uiShop;
    public TextMeshProUGUI text;

    public void Init(UI_Shop uiShop, string text)
    {
        this.uiShop = uiShop;
        this.text.SetText(text);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiShop.OnUseClicked(this);
    }
}
