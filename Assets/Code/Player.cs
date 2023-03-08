using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller c;
    Rigidbody2D rb;
    SpriteRenderer spr;
    BoxCollider2D box;
    ScoreUI scoreUI;

    public float castDist = 1;

    // walking stuff
    public float walkSpeed = 7;
    public float acceleration = 20;
    public float decceleration = 5;
    public float velPower = 1;
    public float frictionAmount = 0.75f;

    // gravity stuff
    public float gravityScale = 5;
    public float fallGravityMultiplier = 1.25f;
    public float fallGravIncreaseRate = 0.25f;

    //Colors
    public Color normalColor;
    public Color hurtColor;
    public Color dashColor;
    public Color stompColor;

    // Dash Indicator
    private DashIndicator dashIndicator;

    public HealthBar healthBar;
    public Health health;
    public Dashable dash;
    public Jumpable jump;
    //public Kickable kick;
    public Stompable stomp;
    public Bullet bullet;

    public Transform t;

    public Transform bulletSpawnPoint;
    public GameObject bulletPrefab;
    public float bulletSpeed = 10;
    private float fireCooldown = .75f;
    public bool canFire = true;

    public bool jumpCooldown;
    public bool dashCooldown;
    public bool stompCooldown;
    public bool puffCooldown;

    // Start is called before the first frame update
    void Start()
    {
        jumpCooldown = false;
        dashCooldown = false;
        stompCooldown = false;
        puffCooldown = false;

        t = GetComponent<Transform>();
        c = GetComponent<Controller>();
        if (c == null)
        {
            c = gameObject.AddComponent<Controller>();
        }

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }

        spr = GetComponent<SpriteRenderer>();

        box = GetComponent<BoxCollider2D>();

        health = GetComponent<Health>();

        bullet = GetComponent<Bullet>();

        //Actions
        dash = GetComponent<Dashable>();
        jump = GetComponent<Jumpable>();
        //kick = GetComponent<Kickable>();
        stomp = GetComponent<Stompable>();

        GameObject maybeScoreUI = GameObject.Find("ScoreUI");
        if (maybeScoreUI != null)
        {
            scoreUI = maybeScoreUI.GetComponent<ScoreUI>();
        }
        dashIndicator = GetComponent<DashIndicator>();
    }

    // Update is called once per frame
    void Update()
    {
        




        if (stomp.isStomping)
        {
            //spr.color = stompColor;
        }
        else if (dash.isDashing)
        {
            spr.color = dashColor;
        }
        else if (health.isInvincible)
        {
            spr.color = hurtColor;
        }
        else
        {
            spr.color = normalColor;

        }

        if (stomp.isStomping || stomp.isSpinning)
        {
            HandleEnemyOverlap();
        }

        if ((Input.GetKeyDown(KeyCode.K) || Input.GetButtonDown("Fire1")) && canFire && World.isRunning) {
            gameObject.SendMessage("DoFire");
            StartCoroutine(Fire());
        }
    }

    private IEnumerator Fire()
    {
        canFire = false;
        Vector2 inputDir = c.GetInputDir();
        var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        var tempGravity = rb.gravityScale;
        rb.gravityScale = 0;
        Vector2 propulsion = new Vector2(-75, -25);

        SoundPlayer.PlaySound(SoundPlayer.Sounds.shoot);

        if (inputDir.magnitude == 0)
        {
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(t.localScale.x, 0) * bulletSpeed;
            rb.AddForce(new Vector2(propulsion.x * t.localScale.x, 0), ForceMode2D.Impulse);
        }
        else
        {
            bullet.GetComponent<Rigidbody2D>().velocity = inputDir * bulletSpeed;
            rb.AddForce(new Vector2(propulsion.x * inputDir.x, propulsion.y * inputDir.y), ForceMode2D.Impulse);
        }

        rb.gravityScale = tempGravity;
        yield return new WaitForSeconds(fireCooldown);
        canFire = true;
    }

    public void StartBleeding()
    {
        health.doBleed = true;
    }

    void FixedUpdate()
    {
        if (!health.isDead())
        {
            if (!dash.isDashing)
            {
                if (!stomp.isStomping)
                {
                    HandleMovement();

                }
                ApplyFriction();
                HandleGravity();
            }
        }
    }

    public void OnDeath()
    {
        rb.simulated = false;
        SoundPlayer.PlaySound(SoundPlayer.Sounds.playerDeath);
        SoundPlayer.StopSound(SoundPlayer.Sounds.background);
    }


    public void HandleMovement()
    {
        //if (!kick.isKicking)
        //{
            Vector2 inputDir = c.GetInputDir();

            if (!(Mathf.Sign(rb.velocity.x) == Mathf.Sign(inputDir.x) && Mathf.Abs(rb.velocity.x) > walkSpeed))
            // skip if we're already moving at faster than max speed and still trying to move in that direction
            {
                // how fast do we want to be going
                float goalSpeed = inputDir.x * walkSpeed;
                // diff between that and how fast we're going now
                float velDif = goalSpeed - rb.velocity.x;
                // how much are we accelerating
                float accelRate = (Mathf.Abs(goalSpeed) > 0.01f) ? acceleration : decceleration;
                // calculate movement force
                float moveForce = Mathf.Pow(Mathf.Abs(velDif) * accelRate, velPower) * Mathf.Sign(velDif);

                rb.AddForce(moveForce * Vector2.right);
            }
       //}
    }

    public void ApplyFriction()
    {
        if (IsGrounded() && Mathf.Abs(c.GetInputDir().x) < Controller.deadzone)
        {
            // use either our velocity or the default friction amt (~.75)
            float fricAmt = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            // set friction to opposite direction of movement
            fricAmt *= -Mathf.Sign(rb.velocity.x);

            rb.AddForce(Vector2.right * fricAmt, ForceMode2D.Impulse);

        }
    }

    public void HandleGravity()
    {
        if (rb.velocity.y < 0)
        {

            rb.gravityScale = Mathf.Lerp(rb.gravityScale,
                                        gravityScale * fallGravityMultiplier,
                                        fallGravIncreaseRate);
        } else
        {
            rb.gravityScale = gravityScale;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        // on collision with the ground
        if (collidingObject.layer == 6 && collision.collider.bounds.center.y < transform.position.y)
        {
            //Debug.Log("refreshing Jumps!");
            if (dash.isDashing)
            {
                dash.StopDash();
            }

            jump.RefreshJumps();
            dash.RefreshDashes();
        }
        else if (collidingObject.name == "Death Plane")
        {
            health.deathTime = Time.time;
        }

        //Debug.Log("COLLIDING WITH ENEMY IN PLAYER");
    }

    public void StartDash()
    {
        HandleEnemyOverlap();
    }

    public void StartKick()
    {
        HandleEnemyOverlap();
    }

    private void HandleEnemyOverlap()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, box.size, 90);
        //Debug.Log("colliders: " + colliders.ToString());
        foreach (Collider2D col in colliders)
        {
            GameObject colObj = col.gameObject;
            //Debug.Log("STARTED OVERLAPPING WITH ENEMY");
            if (colObj.GetComponent<Enemy>() != null)
            {
                DamageEnemy(colObj);
                if (stomp.isStomping || stomp.isSpinning)
                {
                    DamageEnemy(colObj);
                }
                if (dash.isDashing)
                {
                    dash.AddDash();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        if (collidingObject.GetComponent<Enemy>() != null)
        {
            //Debug.Log("COLLIDING WITH ENEMY IN PLAYER");
            if (stomp.isStomping || dash.isDashing || stomp.isSpinning)
            {
                DamageEnemy(collidingObject);

                if (dash.isDashing)
                {
                    dash.AddDash();
                }
            }
        }
        else if (collidingObject.GetComponent<SignPostText>() != null)
        {
            collidingObject.GetComponent<SignPostText>().DisplayText();
            World.PauseGame();
        }
        else if (collidingObject.GetComponent<EnemySpawner>() != null)
        {
            collidingObject.GetComponent<EnemySpawner>().StartSpawn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject collidingObject = collision.gameObject;
        if (collidingObject.GetComponent<SignPostText>() != null)
        {
            collidingObject.GetComponent<SignPostText>().RemoveText();
            World.ContinueGame();
        }
        else if (collidingObject.GetComponent<EnemySpawner>() != null)
        {
            collidingObject.GetComponent<EnemySpawner>().StopSpawn();
        }
    }

    public void DamageEnemy(GameObject enemyObj)
    {
        float damage = dash.dashDmg;
        if (stomp.isStomping || stomp.isSpinning)
        {
            //damage = rb.velocity.magnitude * kick.kickDmgScale;
            
            damage = 100f;
            Debug.Log("Stomp Damage: " + damage);

            
        }
        //Debug.Log("Damage dealt: " + damage);
        Health enemyHealth = enemyObj.GetComponent<Health>();
        float bloodValue = enemyHealth.TakeDamage(damage);
        health.HealDamage(bloodValue);
        if (scoreUI != null)
        {
            scoreUI.Score();

        }
    }

    public bool IsGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        return Physics2D.OverlapBox(box.bounds.center, new Vector2(box.bounds.size.x, box.bounds.size.y), 0, groundMask);
    }


}
