using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using System;

public class Player : Entity
{
    [Header("Weapon")]
    public Weapon weapon;
    public LayerMask projectilLayer;

    [Header("Moviment")]
    public Transform aimTransform;
    public float speed;
    private new Rigidbody2D rigidbody2D;
    private Vector3 moveDir;
    private Vector3 moveChange;
    private Joystick movementJoystick;
    private Joystick aimJoystick;
    private Transform walkableArea;
    private Vector2 maxPosition;
    private Vector2 minPosition;

    [Header("Health")]
    public GameObject damageEffectPrefab;
    public GameObject healthBarPrefab;
    public float health;
    private float maxHealth;

    [Header("Status")]
    public EntityStatus entityStatus;

    [Header("Inventory")]
    public Inventory inventory;

    [Header("Equips")]
    public CharacterEquipment characterEquipment;

    [Header("Animation")]
    public TintAnimation tintAnimation;

    public UI_HealthBar UIHealthBar { get; set; }

    private Transform gfx;
    private Animator gfxAnimation;
    public GameManager GameManager { get; set; }

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Init(UI_HealthBar uiHealthBar, Joystick movementJoystick, Joystick aimJoystick, Transform walkableArea, GameManager gameManager)
    {
        this.UIHealthBar = uiHealthBar;
        this.GameManager = gameManager;
        this.movementJoystick = movementJoystick;
        this.aimJoystick = aimJoystick;
        this.walkableArea = walkableArea;

        maxHealth = health;
        UIHealthBar.setMax(maxHealth);
        UIHealthBar.setValue(maxHealth);
        rigidbody2D = GetComponent<Rigidbody2D>();
        gfx = transform.Find("GFX");
        gfxAnimation = gfx.GetComponent<Animator>();
        state = "IDLE";

        Bounds bounds = walkableArea.GetComponent<Renderer>().bounds;

        maxPosition = new Vector2(bounds.extents.x, bounds.extents.y);
        minPosition = new Vector2(-bounds.extents.x, -bounds.extents.y);

        gameObject.SetActive(true);
    }

    public void Test()
    {
        Item punch = GameManager.GetItem("AK 47");
        inventory.AddItem(punch);
        inventory.TryToUseItem(punch.GetComponent<Weapon>(), 0);
        // characterEquipment.Add(1, GameManager.GetItem("WeaponAk47"));
        // characterEquipment.Add(1, GameManager.GetItem("GrenadeThrower"));
        // characterEquipment.Add(0, GameManager.GetItem("WeaponMelee"));
    }

    new void Update()
    {
        HandleMovement();
        HandleAiming();
        HandleShooting();
        base.Update();
    }

    protected override string UpdateClient()
    {
        /*if (state == "IDLE") return Update_IDLE();
        else if (state == "RUNNING") return Update_RUNNING();*/

        return "IDLE";
    }

    string Update_IDLE()
    {
        if (EventMoveKeyPressed())
        {
            return "RUNNING";
        }

        gfxAnimation.SetBool("run", false);

        return "IDLE";
    }

    string Update_RUNNING()
    {
        if (!EventMoveKeyPressed())
        {
            return "IDLE";
        }

        gfxAnimation.SetBool("run", true);

        return "IDLE";
    }

    bool EventMoveKeyPressed()
    {
        moveChange = Vector3.zero;
        moveChange.x = Input.GetAxisRaw("Horizontal");
        moveChange.y = Input.GetAxisRaw("Vertical");

        return moveChange != Vector3.zero;
    }

    private void HandleMovement()
    {
        /*float moveX = 0f;
        float moveY = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            moveY = +1f;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            moveY = -1f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveX = -1f;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveX = +1f;
        }

        moveDir = new Vector3(moveX, moveY).normalized;*/
         
        moveDir = new Vector3(movementJoystick.Horizontal, movementJoystick.Vertical).normalized;

        gfxAnimation.SetBool("run", moveDir != Vector3.zero);
    }

    private void FixedUpdate()
    {
        Vector2 desiredPosition = transform.position + moveDir * speed * Time.fixedDeltaTime;

        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);

        rigidbody2D.MovePosition(desiredPosition);
    }

    private void HandleAiming()
    {
        aimJoystick.KeepLastPosition = true;
        Vector3 mousePosition = UtilsClass.GetMouseWorldPosition();

        // Vector3 aimDirection = (mousePosition - aimTransform.position).normalized;
        Vector3 aimDirection = new Vector2(aimJoystick.Horizontal, aimJoystick.Vertical).normalized;
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        aimTransform.eulerAngles = new Vector3(0, 0, angle);

        Vector3 aimLocalScale = Vector3.one;
        Quaternion flipGfx = Quaternion.Euler(0, 0, 0);

        if (angle > 90 || angle < -90)
        {
            aimLocalScale.y = -1f;
            flipGfx.y = 180;
        }
        else
        {
            aimLocalScale.y = +1f;
        }
        aimTransform.localScale = aimLocalScale;
        gfx.localRotation = flipGfx;
    }

    private void HandleShooting()
    {
        if (!weapon)
            return;

        if (aimJoystick.OnTouching && Vector2.Distance(aimJoystick.Direction, new Vector2(0f, 0f)) >= 0.7f)
        {
            weapon.Shoot(projectilLayer);
        }

        /*if (Input.GetMouseButton(0))
        {
            if (!weapon)
                return;

            weapon.Shoot(projectilLayer);
        }*/
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        UIHealthBar.setValue(health);
        Instantiate(damageEffectPrefab, transform.position, Quaternion.identity);
        tintAnimation.Begin();
        UtilsClass.ShakeCamera(0.1f, 0.1f);
        checkDeath();
    }

    void checkDeath()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void SetEquipment(GameObject item)
    {
        foreach (Transform child in aimTransform)
        {
            child.gameObject.SetActive(false);
        }

        item.transform.SetParent(aimTransform);
        item.SetActive(true);
        weapon = item.GetComponent<Weapon>();
    }

    public void TryToUseItem(Item item, int slotIndex)
    {
        if (item.tag == "Weapon")
        {
            GameObject itemGO = characterEquipment.AddOnSlot(slotIndex, item);

            if (!weapon)
            {
                SetEquipment(itemGO);
            }

            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Bullet"))
        {
            Bullet projectil = collision.gameObject.GetComponent<Bullet>();

            TakeDamage(DamageHandler.WeaponVsMonster(projectil.weapon, entityStatus));

            return;
        }
    }
}
