using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectil : MonoBehaviour
{
    public new Rigidbody2D rigidbody2D;
    public GameObject impactPrefab;
    public float force;
    public float damageRadius;
    public LayerMask layerToDamage;

    [HideInInspector] public Weapon weapon;
    private float lifeTime = 10f;

    private void Update()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Shoot(Transform bulletSpawnTransform, Weapon weapon)
    {
        this.weapon = weapon;

        rigidbody2D.AddForce(bulletSpawnTransform.right * force, ForceMode2D.Impulse);
    }

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactPrefab)
        {
            Instantiate(impactPrefab, transform.position - new Vector3(0, 0, 1), Quaternion.identity);
        }

        if (damageRadius > 0)
        {
            HandleDamage();
        }

        Destroy(gameObject);
    }

    public void HandleDamage()
    {
        Collider2D[] targetsToDamage = Physics2D.OverlapCircleAll(transform.position, damageRadius, layerToDamage);

        foreach (Collider2D targetToDamage in targetsToDamage)
        {
            if (targetToDamage.CompareTag("Monster"))
            {
                Monster target = targetToDamage.GetComponent<Monster>();
                target.TakeDamage(DamageHandler.WeaponVsMonster(weapon, target.entityStatus));
            }
        }
    }

    public void HandleDamage(Spell spell)
    {
        Collider2D[] targetsToDamage = Physics2D.OverlapCircleAll(transform.position, damageRadius, layerToDamage);
        
        foreach (Collider2D targetToDamage in targetsToDamage)
        {
            if (targetToDamage.CompareTag("Player"))
            {
                Player player = targetToDamage.GetComponent<Player>();
                player.TakeDamage(DamageHandler.PlayerVsMonster(player, spell));
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
