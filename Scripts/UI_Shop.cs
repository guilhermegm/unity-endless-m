using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Shop : MonoBehaviour
{
    [Header("Player")]
    public Player player;

    [Header("Shop")]
    public Shop shop;

    [Header("Tab")]
    public GameObject TabArea;
    public UI_Tab tabPrefab;

    [Header("Page")]
    public GameObject PageArea;
    public GameObject PowerUpArea;
    public GameObject powerUpPrefab;

    [Header("PowerUps")]
    public Sprite damagePowerUpSprite;
    public Sprite fireRatePowerUpSprite;
    public Sprite storedAmmoPowerUpSprite;

    [Header("Stats")]
    public GameObject StatsArea;
    public UI_Stats uiStatsPrefab;

    [Header("Use")]
    public GameObject UseArea;
    public UI_UseButton uiUseButton;

    [Header("Slot")]
    public GameObject SlotArea;

    [Header("Character Equipment")]
    public UI_CharacterEquipment uiCharacterEquipment;

    private List<Weapon> weapons = new List<Weapon>();
    private Weapon weaponSelected;
    private List<WeaponPowerUp> weaponPowerUpsSelected;
    private UI_Tab uiTabSelected;

    public void Init(Shop shop, Player player, UI_CharacterEquipment uiCharacterEquipment)
    {
        this.shop = shop;
        this.player = player;
        this.uiCharacterEquipment = uiCharacterEquipment;
    }

    public void AddTab(Weapon weapon)
    {
        GameObject tab = Instantiate(tabPrefab.gameObject, TabArea.transform);
        UI_Tab uiTab = tab.GetComponent<UI_Tab>();

        uiTab.Init(this, weapon.transform.Find("GFX").GetComponent<SpriteRenderer>().sprite, weapon.name);
        weapons.Add(weapon);
    }

    public void SelectFirstTab()
    {
        UI_Tab tab = GetComponentInChildren<UI_Tab>();
        OnTabSelected(tab);
    }

    private void OnEnable()
    {
        UpdateVisual();
    }

    public void OnTabSelected(UI_Tab uiTab)
    {
        int index = uiTab.transform.GetSiblingIndex();

        UpdateTabSelected(weapons[index], uiTab);
        UpdateVisual();
    }

    public void OnPowerUpClicked(UI_PowerUp uiPowerUp)
    {
        int index = uiPowerUp.transform.GetSiblingIndex();

        shop.TryToBuy(weaponSelected, weaponPowerUpsSelected[index]);
        UpdatePowerUpsVisual();
        UpdateStatsVisual();
        uiCharacterEquipment.UpdateVisual();
    }

    private void AddStats(string textLeft, string textRight)
    {
        GameObject stats = Instantiate(uiStatsPrefab.gameObject, StatsArea.transform);
        UI_Stats uiStats = stats.GetComponent<UI_Stats>();

        uiStats.textLeft.SetText(textLeft);
        uiStats.textRight.SetText(textRight);
    }

    private void ClearPowerUps()
    {
        foreach (Transform child in PowerUpArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ClearStats()
    {
        foreach (Transform child in StatsArea.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void UpdateTabSelected(Weapon weapon, UI_Tab uiTab)
    {
        if (uiTabSelected)
            uiTabSelected.GetComponent<Image>().color = new Color(0.4823529f, 0.4823529f, 0.4823529f, 1f);

        uiTab.GetComponent<Image>().color = new Color(0.9058824f, 0.9058824f, 0.9058824f, 1f);

        weaponSelected = weapon;
        uiTabSelected = uiTab;
    }

    public void OnUseClicked(UI_UseButton uiUseButton)
    {
        SlotArea.SetActive(false);

        if (!player.inventory.GetItemByName(weaponSelected.name))
        {
            shop.TryToBuy(weaponSelected);
            UpdateVisual();
            return;
        }
    }

    public void OnSlotClicked(UI_Slot uiSlot)
    {
        player.inventory.TryToUseItem(weaponSelected, uiSlot.transform.GetSiblingIndex());
        UpdateUseVisual();
    }

    private void UpdateVisual()
    {
        UpdatePowerUpsVisual();
        UpdateStatsVisual();
        UpdateUseVisual();

        if (player.inventory.GetItemByName(weaponSelected.name))
            SlotArea.SetActive(true);
        else
            SlotArea.SetActive(false);
    }

    private void UpdateStatsVisual()
    {
        ClearStats();
        AddStats("Element", weaponSelected.weaponStats.element.ToString());
        AddStats("Attack", weaponSelected.GetMinAttack() + " - " + weaponSelected.GetMaxAttack());
        AddStats("Fire Rate", (1f / weaponSelected.GetFireRate()).ToString("#.#") + "/s");

        WeaponGun weaponGun = weaponSelected.GetComponent<WeaponGun>();

        if (weaponGun)
        {
            AddStats("Ammo", weaponGun.GetMaxStoredAmmo().ToString());
        }

        WeaponThrower weaponThrower = weaponSelected.GetComponent<WeaponThrower>();

        if (weaponThrower)
        {
            AddStats("Ammo", weaponThrower.GetMaxStoredAmmo().ToString());
        }
    }

    private void UpdateUseVisual()
    {
        if (!player.inventory.GetItemByName(weaponSelected.name))
        {
            uiUseButton.gameObject.SetActive(true);
            uiUseButton.Init(this, "Buy\n$" + weaponSelected.price);
            return;
        }

        if (player.inventory.GetItemByName(weaponSelected.name))
        {
            uiUseButton.gameObject.SetActive(false);
            return;
        }
    }

    private void UpdatePowerUpsVisual()
    {
        ClearPowerUps();
        weaponPowerUpsSelected = new List<WeaponPowerUp>();

        if (weaponSelected.weaponDamagePowerUp)
        {
            GameObject powerUp = Instantiate(powerUpPrefab.gameObject, PowerUpArea.transform);
            UI_PowerUp uiPowerUp = powerUp.GetComponent<UI_PowerUp>();

            uiPowerUp.Init(this, damagePowerUpSprite, "Damage", "$" + weaponSelected.weaponDamagePowerUp.GetPrice());
            weaponPowerUpsSelected.Add(weaponSelected.weaponDamagePowerUp);
        }

        if (weaponSelected.weaponFireRatePowerUp)
        {
            GameObject powerUp = Instantiate(powerUpPrefab.gameObject, PowerUpArea.transform);
            UI_PowerUp uiPowerUp = powerUp.GetComponent<UI_PowerUp>();

            uiPowerUp.Init(this, fireRatePowerUpSprite, "Fire Rate", "$" + weaponSelected.weaponFireRatePowerUp.GetPrice());
            weaponPowerUpsSelected.Add(weaponSelected.weaponFireRatePowerUp);
        }

        if (weaponSelected.weaponStoredAmmoPowerUp)
        {
            GameObject powerUp = Instantiate(powerUpPrefab.gameObject, PowerUpArea.transform);
            UI_PowerUp uiPowerUp = powerUp.GetComponent<UI_PowerUp>();

            uiPowerUp.Init(this, storedAmmoPowerUpSprite, "Ammo", "$" + weaponSelected.weaponStoredAmmoPowerUp.GetPrice());
            weaponPowerUpsSelected.Add(weaponSelected.weaponStoredAmmoPowerUp);
        }
    }
}
