using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell : Projectil
{
    public WeaponStats weaponStats;

    public void Init(LayerMask layerMask, LayerMask layerToDamage)
    {
        this.layerToDamage = layerToDamage;
        gameObject.layer = GameManager.LayerMaskToLayer(layerMask);
    }

    public void Cast(Vector2 direction)
    {
        rigidbody2D.AddForce(direction.normalized * force, ForceMode2D.Impulse);
    }

    public void Cast(Transform target)
    {
        Cast(target.position - transform.position);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (impactPrefab)
        {
            Instantiate(impactPrefab, transform.position - new Vector3(0, 0, 1), Quaternion.identity);
        }

        if (damageRadius > 0)
        {
            HandleDamage(this);
        }

        Destroy(gameObject);
    }
}
