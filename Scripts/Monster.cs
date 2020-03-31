using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using Pathfinding;
using TMPro;

public class Monster : Entity
{
    [Header("Status")]
    public EntityStatus entityStatus;
    public float speed;

    [Header("Health")]
    public float health;
    private float maxHealth;
    public GameObject damageEffectPrefab;
    public TextMeshPro damagePopupPrefab;
    public GameObject[] groundBloodsPrefab;

    [Header("Combat")]
    public float attackDelay;
    public float attackRange;
    public LayerMask projectilLayer;

    [Header("Melee Combat")]
    public float attackRadius;
    public float attackDamage;
    public float damageDelay;
    public GameObject meleeAttackEffect;

    [Header("Ranged Weapon")]
    public Transform aim;
    public Weapon rangedWeapon;
    public float rangedAttackDistance;

    [Header("Spells")]
    public SpellHandler spellHandler;

    [Header("Loot")]
    public LootTable lootTable;

    [Header("Animation")]
    public TintAnimation tintAnimation;

    private Transform gfx;
    private Animator gfxAnimation;
    private GameManager gameManager;
    private float timeBtwAttack;
    private GameObject target;
    private Vector3 attackPosition;

    void Awake()
    {
        maxHealth = health;
        state = "CHASING";
        gfx = transform.Find("GFX");
        gfxAnimation = gfx.GetComponent<Animator>();
    }

    private void Start()
    {
        gameManager = GameManager.instance;

        // spellHandler.CastCross(this);
    }

    protected override string UpdateClient()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag("Player");

            // if (target)
                // aIDestinationSetter.target = target.transform;

            return "IDLE";
        }

        HandleAiming();
        HandleRangedWeaponAiming();

        if (state == "IDLE") return Update_IDLE();
        else if (state == "CHASING") return Update_CHASING();
        else if (state == "ATTACKING") return Update_ATTACKING();
        else if (state == "CASTING") return Update_CASTING();

        return "CHASING";
    }

    string Update_IDLE()
    {
        if (rangedAttackDistance > 0 && !EventTargetInRangedDistance())
        {
            return "CHASING";
        }

        if (rangedAttackDistance <= 0 && !EventTargetAttackInRange())
        {
            return "CHASING";
        }

        if (EventCanCastSpell())
        {
            return "CASTING";
        }

        if (EventTargetAttackInRange())
        {
            return "ATTACKING";
        }

        gfxAnimation.SetBool("run", false);

        return "IDLE";
    }

    string Update_CHASING()
    {
        if (EventCanCastSpell()) {
            return "CASTING";
        }

        if (EventTargetAttackInRange())
        {
            return "ATTACKING";
        }

        if (rangedAttackDistance > 0 && EventTargetInRangedDistance())
        {
            return "IDLE";
        }

        transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);
        gfxAnimation.SetBool("run", true);

        return "CHASING";
    }

    string Update_ATTACKING()
    {
        if (EventCanCastSpell())
        {
            return "CASTING";
        }

        if (EventTargetAttackInRange())
        {
            gfxAnimation.SetBool("run", false);
            HandleAttack();
            return "ATTACKING";
        }

        return "IDLE";
    }

    string Update_CASTING()
    {
        if (EventIsCastingSpell())
        {
            gfxAnimation.SetBool("run", false);
            return "CASTING";
        }

        if (EventCanCastSpell())
        {
            HandleCastSpell();
            return "CASTING";
        }

        return "IDLE";
    }

    private bool EventTargetAttackInRange()
    {
        float distanceFromTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceFromTarget <= attackRange;
    }

    public bool EventTargetInRangedDistance()
    {
        float distanceFromTarget = Vector3.Distance(target.transform.position, transform.position);
        return distanceFromTarget <= rangedAttackDistance;
    }

    private bool EventCanCastSpell()
    {
        return EventTargetInRangedDistance() && spellHandler && spellHandler.canCast;
    }

    private bool EventIsCastingSpell()
    {
        return spellHandler && spellHandler.isCasting;
    }

    private void HandleAiming()
    {
        Vector3 lookDirection = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;

        Quaternion flipGfx = Quaternion.Euler(0, 0, 0);

        if (angle > 90 || angle < -90)
        {
            flipGfx.y = 180;
        }

        gfx.localRotation = flipGfx;
    }

    private void HandleRangedWeaponAiming()
    {
        if (!(aim && rangedWeapon))
            return;

        Vector3 aimDirection = (target.transform.position - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Vector3 rotation = Quaternion.Lerp(aim.rotation, Quaternion.LookRotation(aimDirection), Time.deltaTime * 3f).eulerAngles;
        aim.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        Quaternion flipGfx = Quaternion.Euler(0, 0, 0);

        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
        }
        else
        {
            aimLocalScale.y = +1f;
        }
        aim.localScale = aimLocalScale;
    }

    private void HandleCastSpell()
    {
        spellHandler.Cast(this, target.transform);
    }

    void HandleAttack()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack -= Time.deltaTime;
            return;
        }

        if (rangedWeapon)
        {
            Shoot();
        } else
        {
            Invoke("Attack", damageDelay);
        }

        timeBtwAttack = attackDelay;
    }

    private void Shoot()
    {
        rangedWeapon.Shoot(projectilLayer);
    }

    void Attack()
    {
        StartCoroutine(AttackPosition(target.transform.position));
    }

    IEnumerator AttackPosition(Vector3 attackPosition)
    {
        yield return new WaitForSeconds(damageDelay);

        this.attackPosition = attackPosition;

        Collider2D[] targetsToDamage = Physics2D.OverlapCircleAll(attackPosition, attackRadius, LayerMask.GetMask("Player"));

        gfxAnimation.SetBool("attack", true);
        Instantiate(meleeAttackEffect, attackPosition, Quaternion.identity);
        
        foreach (Collider2D targetToDamage in targetsToDamage)
        {
            if (targetToDamage.GetComponent<Player>() is Player)
            {
                targetToDamage.GetComponent<Player>().TakeDamage(attackDamage);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        if (tintAnimation)
            tintAnimation.Begin();

        TextMeshPro damagePopup = Instantiate(damagePopupPrefab, transform.position, Quaternion.identity);
        damagePopup.SetText(((int)damage).ToString());

        CheckDeath();
    }

    void CheckDeath()
    {
        if (health <= 0)
        {
            DropItems();
            GameManager gameManager = GameObject.FindObjectOfType<GameManager>();
            Debug.Log("1");
            Debug.Log(gameManager);
            Debug.Log("2");
            Debug.Log(GameManager.instance);
            Debug.Log("3");
            Debug.Log(GameManager.instance.waveManager);
            gameManager.waveManager.OnMonsterSpawnedDeath(gameObject);
            UtilsClass.ShakeCamera(0.03f, 0.1f);

            GameObject groundBlood = groundBloodsPrefab[Random.Range(0, groundBloodsPrefab.Length)];
            Instantiate(groundBlood, transform.position, groundBlood.transform.rotation);

            Destroy(gameObject);
        }
    }

    private void DropItems()
    {
        if (!lootTable)
            return;

        ArrayList drops = lootTable.Drops();

        foreach (Loot loot in drops)
        {
            gameManager.player.inventory.AddItem(loot.item, loot.Quantity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Bullet projectil = collision.gameObject.GetComponent<Bullet>();

        if (projectil is Bullet)
        {
            TakeDamage(DamageHandler.WeaponVsMonster(projectil.weapon, entityStatus));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(attackPosition, attackRadius);
    }
}
