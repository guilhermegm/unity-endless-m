using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    public event EventHandler<OnEquipmentChangedEventArgs> OnEquipmentChanged;
    public class OnEquipmentChangedEventArgs : EventArgs
    {
        public GameObject itemGO;
        public int index;
    }
    public GameObject[] items;
    [SerializeField] private Player player;

    public void AddOnSlot(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i])
                continue;

            AddOnSlot(i, item);
        }
    }

    public GameObject AddOnSlot(int slotNumber, Item item)
    {
        GameObject itemGO;

        int itemEquipedIndex = Array.FindIndex(items, itemEquiped => {
            return itemEquiped && itemEquiped.name.Replace("(Clone)", "") == item.gameObject.name;
        });

        if (itemEquipedIndex >= 0)
            itemGO = items[itemEquipedIndex];
        else
            itemGO = Instantiate(item.gameObject, player.aimTransform);

        if (itemEquipedIndex >= 0 && itemEquipedIndex != slotNumber)
        {
            items[itemEquipedIndex] = null;
            OnEquipmentChanged?.Invoke(this, new OnEquipmentChangedEventArgs { itemGO = null, index = itemEquipedIndex });
        }

        itemGO.SetActive(false);
        items[slotNumber] = itemGO;

        OnEquipmentChanged?.Invoke(this, new OnEquipmentChangedEventArgs { itemGO = itemGO, index = slotNumber });

        return itemGO;
    }

    public void TryEquip(int slotIndex, UI_CharacterEquipmentSlot uiCharacterEquipmentSlot)
    {
        if (!items[slotIndex])
            return;

        player.SetEquipment(items[slotIndex]);
    }
}
