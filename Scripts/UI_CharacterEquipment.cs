using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CharacterEquipment : MonoBehaviour
{
    public UI_CharacterEquipmentSlot[] uiCharacterEquipmentSlots;
    private CharacterEquipment characterEquipment;

    public void Init(CharacterEquipment characterEquipment)
    {
        this.characterEquipment = characterEquipment;
        characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;

        uiCharacterEquipmentSlots = GetComponentsInChildren<UI_CharacterEquipmentSlot>();

        foreach (UI_CharacterEquipmentSlot slot in uiCharacterEquipmentSlots)
        {
            slot.OnSlotClicked += UI_CharacterEquipmentSlot_OnSlotClicked;
        }
    }

    private void UI_CharacterEquipmentSlot_OnSlotClicked(object sender, UI_CharacterEquipmentSlot.OnSlotClickedEventArgs e)
    {
        characterEquipment.TryEquip(e.slotIndex, (UI_CharacterEquipmentSlot)sender);
    }

    public void SetCharacterEquipment(CharacterEquipment characterEquipment)
    {
        this.characterEquipment = characterEquipment;
        characterEquipment.OnEquipmentChanged += CharacterEquipment_OnEquipmentChanged;
    }

    private void CharacterEquipment_OnEquipmentChanged(object sender, CharacterEquipment.OnEquipmentChangedEventArgs e)
    {
        UpdateVisual();
    }

    public void UpdateVisual()
    {
        for (int i = 0; i < characterEquipment.items.Length; i++)
        {
            if (!characterEquipment.items[i])
            {
                uiCharacterEquipmentSlots[i].Remove();
                continue;
            }

            uiCharacterEquipmentSlots[i].Add(characterEquipment.items[i], i);
        }
    }
}
