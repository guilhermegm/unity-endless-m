using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Throwable : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    public GameObject impactPrefab;
    public float countdownToExplode;
    public float force;
    public float damageRadius;
    [HideInInspector] public Weapon weapon;

    private void Update()
    {
        HandleExplosion();
    }

    void HandleExplosion()
    {
        countdownToExplode -= Time.deltaTime;

        if (countdownToExplode <= 0)
        {
            Instantiate(impactPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
            HandleDamage();
        }
    }

    void HandleDamage()
    {
        Collider2D[] targetsToDamage = Physics2D.OverlapCircleAll(transform.position, damageRadius, LayerMask.GetMask("Monster"));

        foreach (Collider2D targetToDamage in targetsToDamage)
        {
            Monster target = targetToDamage.GetComponent<Monster>();

            if (target is Monster)
            {
                target.TakeDamage(DamageHandler.WeaponVsMonster(weapon, target.entityStatus));
            }
        }
    }

    public void Shoot(Transform spawnTransform, Weapon weapon)
    {
        this.weapon = weapon;

        rigidbody2D.AddForce(spawnTransform.right * force, ForceMode2D.Impulse);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
