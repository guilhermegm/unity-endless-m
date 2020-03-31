using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponThrower : Weapon
{
    public Throwable throwablePrefab;
    
    [Header("Ammo")]
    public int maxStoredAmmo;
    public int storedAmmo;

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public WeaponThrower weaponThrower;
    }
    public event EventHandler<OnReloadEventArgs> OnReload;
    public class OnReloadEventArgs : EventArgs
    {
        public WeaponThrower weaponThrower;
    }

    private void Awake()
    {
        RechargeAllAmmo();
    }

    public override string UpdateClient()
    {
        if (state == "IDLE") return Update_IDLE();
        else if (state == "SHOOTING") return Update_SHOOTING();

        return "IDLE";
    }

    private string Update_IDLE()
    {
        return "IDLE";
    }

    private string Update_SHOOTING()
    {
        if (EventOutOfAmmo())
        {
            return "IDLE";
        }

        if (EventCanShoot())
        {
            canShoot = false;
            fireRateTimer = GetFireRate();
            HandleShoot();

            return "IDLE";
        }

        return "SHOOTING";
    }

    private bool EventOutOfAmmo()
    {
        return storedAmmo <= 0;
    }

    private bool EventCanShoot()
    {
        return canShoot;
    }

    public override void HandleShoot()
    {
        Throwable throwable = Instantiate(throwablePrefab, transform.position, Quaternion.identity);
        throwable.Shoot(transform, this);
        storedAmmo--;
        OnShoot?.Invoke(this, new OnShootEventArgs { weaponThrower = this });
    }

    public void RechargeAllAmmo()
    {
        storedAmmo = GetMaxStoredAmmo();
        OnReload?.Invoke(this, new OnReloadEventArgs { weaponThrower = this });
    }

    public int GetMaxStoredAmmo()
    {
        float storedAmmoMod = weaponStoredAmmoPowerUp ? weaponStoredAmmoPowerUp.GetTotalBonusPercent() : 0f;
        return (int)(maxStoredAmmo * (1 + storedAmmoMod));
    }
}
