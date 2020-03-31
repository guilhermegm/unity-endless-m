using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class WeaponGun : Weapon
{
    [HideInInspector] public GameManager gameManager;
    public Bullet bulletPrefab;
    public Transform bulletSpawnTransform;
    public ParticleSystem muzzleFlashs;
    public int magazineSize;
    public int maxStoredAmmo;
    public float reloadingTime;
    
    private float reloadingTimeTimer;
    [HideInInspector] public int magazineAmmo;
    [HideInInspector] public int storedAmmo;
    private Reloading reloadingState;
    private enum Reloading { WAITING, DONE };

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public WeaponGun weaponGun;
    }
    public event EventHandler<OnReloadEventArgs> OnReload;
    public class OnReloadEventArgs : EventArgs
    {
        public WeaponGun weaponGun;
    }

    private new void Awake()
    {
        base.Awake();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        reloadingTimeTimer = reloadingTime;
        reloadingState = Reloading.WAITING;
        RechargeAllAmmo();
    }

    private void Start()
    {

    }

    private new void Update()
    {
        UpdateReloadingTimer();
        base.Update();
    }

    private void UpdateReloadingTimer()
    {
        if (state != "RELOADING")
            return;

        reloadingTimeTimer -= Time.deltaTime;
        gameManager.uiReloading.setValue((reloadingTime - reloadingTimeTimer) / reloadingTime);

        if (reloadingTimeTimer > 0)
            return;

        reloadingTimeTimer = reloadingTime;
        reloadingState = Reloading.DONE;
        state = "RELOADING";
    }

    public override string UpdateClient()
    {
        if (state == "IDLE") return Update_IDLE();
        else if (state == "SHOOTING") return Update_SHOOTING();
        else if (state == "RELOADING") return Update_RELOADING();

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

        if (EventOutOfBulletsInMagazine())
        {
            return "RELOADING";
        }

        if (EventCanShoot())
        {
            canShoot = false;
            fireRateTimer = GetFireRate();
            HandleShoot();

            if (EventOutOfBulletsInMagazine())
            {
                return "RELOADING";
            }

            return "IDLE";
        }

        return "SHOOTING";
    }

    private string Update_RELOADING()
    {
        if (reloadingState == Reloading.DONE)
        {
            ReloadMagazine();
            reloadingState = Reloading.WAITING;
            gameManager.uiReloading.setValue(0f);

            return "IDLE";
        }

        if (EventIsReloading())
        {
            return "RELOADING";
        }

        if (EventOutOfBulletsInMagazine())
        {
            return "RELOADING";
        }

        return "IDLE";
    }

    private bool EventCanShoot()
    {
        return canShoot;
    }

    private bool EventOutOfAmmo()
    {
        return storedAmmo <= 0 && magazineAmmo <= 0;
    }

    private bool EventOutOfBulletsInMagazine()
    {
        return magazineAmmo <= 0;
    }

    public bool EventIsReloading()
    {
        return state == "RELOADING";
    }

    public override void HandleShoot()
    {
        Bullet bullet = Instantiate(bulletPrefab, bulletSpawnTransform.position, bulletSpawnTransform.rotation);
        bullet.gameObject.layer = GameManager.LayerMaskToLayer(projectilLayer);
        bullet.Shoot(bulletSpawnTransform, this);
        if (muzzleFlashs)
            muzzleFlashs.Play();
        magazineAmmo--;
        OnShoot?.Invoke(this, new OnShootEventArgs { weaponGun = this });
    }

    private void ReloadMagazine()
    {
        int bulletsToReload = storedAmmo < magazineSize ? storedAmmo : magazineSize;

        storedAmmo -= bulletsToReload;
        magazineAmmo = bulletsToReload;
        OnReload?.Invoke(this, new OnReloadEventArgs { weaponGun = this });
    }

    private void RechargeAllAmmo()
    {
        storedAmmo = GetMaxStoredAmmo();
        magazineAmmo = magazineSize;
        OnReload?.Invoke(this, new OnReloadEventArgs { weaponGun = this });
    }

    public int GetMaxStoredAmmo()
    {
        float storedAmmoMod = weaponStoredAmmoPowerUp ? weaponStoredAmmoPowerUp.GetTotalBonusPercent() : 0f;
        return (int)(maxStoredAmmo * (1 + storedAmmoMod));
    }
}
