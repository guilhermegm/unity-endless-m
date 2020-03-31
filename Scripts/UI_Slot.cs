using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Slot : MonoBehaviour, IPointerClickHandler
{
    public UI_Shop uiShop;

    public void OnPointerClick(PointerEventData eventData)
    {
        uiShop.OnSlotClicked(this);
    }
}
