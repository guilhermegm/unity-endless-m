using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private int golds;
    public List<Item> items;

    public event EventHandler<OnGoldChangeEventArgs> OnGoldChange;
    public class OnGoldChangeEventArgs : EventArgs
    {
        public int golds;
    }

    private void Awake()
    {
        golds = 11000000;
    }

    public void AddItem(Item item, int quantity = 1)
    {
        if (item.name == "Gold")
        {
            golds += quantity;
            OnGoldChange?.Invoke(this, new OnGoldChangeEventArgs { golds = this.golds });

            return;
        }

        if (item.tag == "Weapon")
        {
            Debug.Log("Add weapon " + item);
            items.Add(item);

            return;
        }
    }

    public int GetGold()
    {
        return golds;
    }

    public void SpendGold(int gold)
    {
        golds -= gold;
        OnGoldChange?.Invoke(this, new OnGoldChangeEventArgs { golds = this.golds });
    }

    public void TryToUseItem(Weapon weapon, int slotIndex)
    {
        player.TryToUseItem(GetItemByName(weapon.name), slotIndex);
    }

    public List<Item> GetWeapons()
    {
        return items.FindAll(item => item.tag == "Weapon");
    }

    public Item GetItemByName(string name)
    {
        return items.Find(item => item.name == name);
    }
}
