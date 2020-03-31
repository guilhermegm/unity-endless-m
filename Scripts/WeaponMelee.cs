using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponMelee : Weapon
{
    public float attackRadius;
    public float attackRange;
    public Animator attackAnimator;

    [Header("Impact")]
    public Transform impactSpawnPoint;
    public GameObject impactEffectPrefab;
    
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
        if (EventCanShoot())
        {
            canShoot = false;
            fireRateTimer = GetFireRate();
            HandleShoot();

            return "IDLE";
        }

        return "SHOOTING";
    }

    private bool EventCanShoot()
    {
        return canShoot;
    }

    public override void HandleShoot()
    {
        attackAnimator.Play("MeleeAttack");
        Invoke("DisplayImpactEffect", 0.1f);
        HandleDamage();
    }

    private void DisplayImpactEffect()
    {
        GameObject impact = Instantiate(impactEffectPrefab, impactSpawnPoint.position, Quaternion.identity);
        Destroy(impact, 0.3f);
    }

    void HandleDamage()
    {
        Collider2D[] targetsToDamage = Physics2D.OverlapCircleAll(
            transform.position + transform.right * attackRange,
            attackRadius,
            LayerMask.GetMask("Monster")
        );

        foreach (Collider2D targetToDamage in targetsToDamage)
        {
            Monster target = targetToDamage.GetComponent<Monster>();

            if (target is Monster)
            {
                target.TakeDamage(DamageHandler.WeaponVsMonster(this, target.entityStatus));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.right * attackRange, attackRadius);
    }
}
