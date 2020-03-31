using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    [Header("Stats")]
    public WeaponStats weaponStats;
    public WeaponDamagePowerUp weaponDamagePowerUp;
    public WeaponFireRatePowerUp weaponFireRatePowerUp;
    public WeaponStoredAmmoPowerUp weaponStoredAmmoPowerUp;

    [Header("Combat")]
    public float fireRate;
    [HideInInspector] public float fireRateTimer;
    [HideInInspector] public bool canShoot;
    [HideInInspector] public string state;
    [HideInInspector] public LayerMask projectilLayer;

    [Header("Shop")]
    public int price;

    public void Awake()
    {
        fireRateTimer = GetFireRate();
        canShoot = true;
    }

    public void Shoot(LayerMask projectilLayer)
    {
        this.projectilLayer = projectilLayer;

        if (!canShoot || state == "RELOADING")
            return;

        state = "SHOOTING";
    }

    public virtual void HandleShoot()
    {

    }

    public virtual string UpdateClient()
    {
        // if (state == "IDLE") return Update_IDLE();

        return "IDLE";
    }

    private string Update_IDLE()
    {
        return "IDLE";
    }

    private bool EventCanShoot()
    {
        return canShoot;
    }

    public void Update()
    {
        state = UpdateClient();
        UpdateShooting();
    }

    public void UpdateShooting()
    {
        if (canShoot)
            return;

        fireRateTimer -= Time.deltaTime;

        if (fireRateTimer > 0)
            return;

        canShoot = true;
    }

    public int GetMinAttack()
    {
        float bonus = weaponDamagePowerUp ? weaponDamagePowerUp.GetTotalBonusPercent() : 0f;

        return (int)(weaponStats.minAttack * (1 + bonus));
    }

    public int GetMaxAttack()
    {
        float bonus = weaponDamagePowerUp ? weaponDamagePowerUp.GetTotalBonusPercent() : 0f;

        return (int)(weaponStats.maxAttack * (1 + bonus));
    }

    public float GetFireRate()
    {
        float bonus = weaponFireRatePowerUp ? weaponFireRatePowerUp.GetTotalBonusPercent() : 0f;

        return fireRate - (fireRate * bonus);
    }
}
