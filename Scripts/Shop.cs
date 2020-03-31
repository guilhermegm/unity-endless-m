using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<Weapon> weapons;
    [HideInInspector] public Player player;
    [HideInInspector] public UI_Shop uiShop;

    public void Init(Player player, UI_Shop uiShop)
    {
        this.player = player;
        this.uiShop = uiShop;

        foreach (Weapon weapon in weapons)
        {
            uiShop.AddTab(weapon);
        }

        uiShop.SelectFirstTab();
    }

    public void TryToBuy(Weapon weapon)
    {
        if (player.inventory.GetItemByName(weapon.name))
        {
            // Already bought
            return;
        }

        if (player.inventory.GetGold() < weapon.price)
        {
            // Not enought money
            return;
        }

        player.inventory.SpendGold(weapon.price);
        player.inventory.AddItem(weapon);
    }

    public void TryToBuy(Weapon weapon, WeaponPowerUp weaponDamagePowerUp)
    {
        if (!player.inventory.GetItemByName(weapon.name))
        {
            // Not bought
            return;
        }

        if (player.inventory.GetGold() < weaponDamagePowerUp.GetPrice())
        {
            // Not enought money
            return;
        }

        player.inventory.SpendGold(weaponDamagePowerUp.GetPrice());
        weaponDamagePowerUp.Upgrade();
    }
}
