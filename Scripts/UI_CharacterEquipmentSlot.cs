using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_CharacterEquipmentSlot : MonoBehaviour
{
    public event EventHandler<OnSlotClickedEventArgs> OnSlotClicked;
    public class OnSlotClickedEventArgs : EventArgs
    {
        public GameObject item;
        public int slotIndex;
    }

    public Image icon;
    public TextMeshProUGUI bottomText;
    [HideInInspector] public GameObject item;
    private int slotIndex;

    public void Add(GameObject item, int slotIndex)
    {
        if (this.item && this.item.name != item.name)
            unsubWeaponEvents();

        this.item = item;
        this.slotIndex = slotIndex;
        icon.sprite = item.transform.Find("GFX").GetComponent<SpriteRenderer>().sprite;
        icon.enabled = true;

        subWeaponEvents();
        UpdateVisual();
    }

    public void Remove()
    {
        unsubWeaponEvents();

        item = null;
        icon.sprite = null;
        icon.enabled = false;

        ClearText();
        UpdateVisual();
    }

    public void OnClick()
    {
        if (item)
            OnSlotClicked?.Invoke(this, new OnSlotClickedEventArgs { item = this.item, slotIndex = this.slotIndex });
    }

    public void WeaponGun_OnShoot(object sender, WeaponGun.OnShootEventArgs e)
    {
        Debug.Log("WeaponGun_OnShoot");
        UpdateVisualText(e.weaponGun);
    }

    public void WeaponGun_OnReload(object sender, WeaponGun.OnReloadEventArgs e)
    {
        UpdateVisualText(e.weaponGun);
    }

    public void WeaponThrower_OnShoot(object sender, WeaponThrower.OnShootEventArgs e)
    {
        UpdateVisualText(e.weaponThrower);
    }

    public void WeaponThrower_OnReload(object sender, WeaponThrower.OnReloadEventArgs e)
    {
        UpdateVisualText(e.weaponThrower);
    }

    public void UpdateVisualText(WeaponGun weaponGun)
    {
        bottomText.SetText(weaponGun.magazineAmmo + "/" + weaponGun.storedAmmo);
    }

    public void UpdateVisualText(WeaponThrower weaponThrower)
    {
        bottomText.SetText(weaponThrower.storedAmmo.ToString());
    }

    private void ClearText()
    {
        bottomText.SetText("");
    }

    private void subWeaponEvents()
    {
        if (!item)
            return;

        WeaponGun weaponGun = item.GetComponent<WeaponGun>();
        if (weaponGun)
        {
            Debug.Log("Sub " + item.GetInstanceID() + " wp " + weaponGun);

            weaponGun.OnShoot += WeaponGun_OnShoot;
            weaponGun.OnReload += WeaponGun_OnReload;
            return;
        }

        WeaponThrower weaponThrower = item.GetComponent<WeaponThrower>();

        if (weaponThrower)
        {
            weaponThrower.OnShoot += WeaponThrower_OnShoot;
            weaponThrower.OnReload += WeaponThrower_OnReload;
            return;
        }
    }

    private void unsubWeaponEvents()
    {
        if (!item)
            return;
        Debug.Log("Unsub");
        WeaponGun weaponGun = item.GetComponent<WeaponGun>();

        if (weaponGun)
        {
            weaponGun.OnShoot -= WeaponGun_OnShoot;
            weaponGun.OnReload -= WeaponGun_OnReload;
            return;
        }

        WeaponThrower weaponThrower = item.GetComponent<WeaponThrower>();

        if (weaponThrower)
        {
            weaponThrower.OnShoot -= WeaponThrower_OnShoot;
            weaponThrower.OnReload -= WeaponThrower_OnReload;
            return;
        }
    }

    private void UpdateVisual()
    {
        if (!item)
            return;

        WeaponGun weaponGun = item.GetComponent<WeaponGun>();

        if (weaponGun)
        {
            UpdateVisualText(weaponGun);
            bottomText.gameObject.SetActive(true);

            return;
        }

        WeaponThrower weaponThrower = item.GetComponent<WeaponThrower>();

        if (weaponThrower)
        {
            UpdateVisualText(weaponThrower);
            bottomText.gameObject.SetActive(true);

            return;
        }

        bottomText.SetText("");
        bottomText.gameObject.SetActive(false);
    }
}
