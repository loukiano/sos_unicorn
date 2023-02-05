using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller c;
    Rigidbody2D rb;
    SpriteRenderer spr;
    BoxCollider2D box;
    GameObject scoreUI;

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

    // jumping stuff
    public int numJumps = 2;
    private int jumpsLeft = 2;
    private float lastJump = 0; // when was the player's last jump
    private bool isJumping = false;
    public float jumpCooldown = 0f; // seconds
    public float jumpForce = 25;
    public float jumpStopMultiplier = 0.5f;

    // dash variables
    public float dashingPowerX = 20f;
    public float dashingPowerY = 20f;
    private bool canDash = true;
    private bool isDashing;
    public float dashingTime = 0.2f;
    public float dashingCooldown = 0.5f;
    public float dashDmg = 100;

    // enemy contact variables
    private bool isInvincible;
    public float invincibleTime = 3.0f;

    //kick variables
    public float kickCooldown;
    public float kickDuration;
    public float kickVel;
    public float kickDmgScale = 1;
    public Vector2 kickSize;
    public bool isKicking;
    private bool canKick;

    //Colors
    public Color normalColor;
    public Color hurtColor;
    public Color dashColor;
    public Color kickColor;

    public float health, maxHealth;
    public HealthBar healthBar;

    public bool isDead;

    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = 100;
        isDead = false;

        canKick = true;
        kickCooldown = 2;
        kickVel = 7;
        kickDuration = 0.1f;
        kickSize = new Vector2(1.5f, 3);

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

        scoreUI = GameObject.Find("ScoreUI");

        normalColor = new Color(46f/255f, 173f/255f, 94f/255f);
        hurtColor = new Color(165f / 255f, 250f / 255f, 198f / 255f);
        dashColor = new Color(1f, 13f/255f, 0f);
        kickColor = new Color(1f, 13f/255f, 0f);
    }

    // Update is called once per frame
    void Update()
    {   
        if (health <= 0)
        {
            isDead = true;
        }

        if (health > 0)
        {
            health -= 1 / 120f;
            healthBar.UpdateHealthBar();
            if (!isDead)
            {
                if (isKicking)
                {
                    spr.color = kickColor;
                }
                else if (isDashing)
                {
                    spr.color = dashColor;
                }
                else if (isInvincible)
                {
                    spr.color = hurtColor;
                }
                else
                {
                    spr.color = normalColor;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    //HealDamage();
                    // TakeDamage();
                }

                if (health > 0)
                {
                    health -= 1 / 120f;
                    healthBar.UpdateHealthBar();
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            if (!isDashing)
            {
                HandleMovement();
                ApplyFriction();
                HandleGravity();
            }
        }      
    }

    public IEnumerator TakeDamage(float dmg)
    {
        // Use your own damage handling code, or this example one.
        health -= dmg;
        healthBar.UpdateHealthBar();
        isInvincible = true;
        Debug.Log("isInvincible: " + isInvincible);
        yield return new WaitForSeconds(dashingTime);
        isInvincible = false;
    }

    public void HealDamage(float healAmt)
    {
        health += healAmt;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        healthBar.UpdateHealthBar();
        scoreUI.GetComponent<ScoreUI>().Score();
    }

    public void HandleMovement()
    {
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

    // called via messages by the Controller
    public void DoJump()
    {
        if (Time.time > lastJump + jumpCooldown && jumpsLeft > 0)
        {
            if (isDashing)
            {
                isDashing = false;
            }
            //Debug.Log("Jumping!");
            lastJump = Time.time;
            jumpsLeft -= 1;
            isJumping = true;

            if (rb.velocity.y < 0)
                // if falling
            {

                rb.AddForce(Vector2.down * rb.velocity.y, ForceMode2D.Impulse);
            }

            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    public void DoDash()
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    // called via messages by the Controller
    public void StopJump()
    {
        if (rb.velocity.y > 0 && isJumping)
        {
            //Debug.Log("Stopping jump!");
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpStopMultiplier), ForceMode2D.Impulse);

        }
    }

    public void DoKick()
    {
        if (canKick)
        {
            StartCoroutine(Kick());

        }
    }

    private IEnumerator Kick()
    {

        canKick = false;
        isKicking = true;

        Vector2 boxSize = box.size;
        box.size = kickSize;

        if (isDashing)
        {
            Debug.Log("adding velocity!");

            Vector2 addVel = rb.velocity;
            addVel.Normalize();
            addVel *= kickVel;
            rb.velocity += addVel;
        }

        yield return new WaitForSeconds(kickDuration);

        box.size = boxSize;
        
        isKicking = false;

        yield return new WaitForSeconds(kickCooldown);
        canKick = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        // on collision with the ground
        if (collidingObject.layer == 6 && collision.collider.bounds.center.y < transform.position.y && jumpsLeft < numJumps)
        {
            //Debug.Log("refreshing Jumps!");
            if (isDashing)
            {
                isDashing = false;
            }

            isJumping = false;
            jumpsLeft = numJumps;
        }
        else if (collidingObject.name == "Death Plane")
        {
            health = 0;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        GameObject collidingObject = collision.gameObject;
        //Debug.Log("HIT SOMETHING");
        if (collidingObject.GetComponent<Enemy>())
        {
            if (isKicking || isDashing)
            {
                float damage = dashDmg;
                if (isKicking)
                {
                    damage = rb.velocity.magnitude * kickDmgScale;
                }
                float lifesteal = collidingObject.GetComponent<Enemy>().TakeDamage(damage);
                HealDamage(lifesteal);
            }
            else if (!isInvincible)
            {
                Debug.Log("Taking Damage");
                StartCoroutine(TakeDamage(collidingObject.GetComponent<Enemy>().GetAtkDmg()));
            }
        }
    }

    private bool IsGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        return Physics2D.OverlapBox(box.bounds.center, new Vector2(box.bounds.size.x, box.bounds.size.y), 0, groundMask);
    }

    private IEnumerator Dash()
    {
        Vector2 inputDir = c.GetInputDir();

        canDash = false;
        isDashing = true;
        
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float newXVel = inputDir.x * dashingPowerX;
        if (Mathf.Sign(inputDir.x) == Mathf.Sign(rb.velocity.x))
            // conserve momentum if same direction
        {
            newXVel += rb.velocity.x;
        }

        float newYVel = inputDir.y * dashingPowerY;
        if (Mathf.Sign(inputDir.y) == Mathf.Sign(rb.velocity.y))
        {
            newYVel += rb.velocity.y;
        }

        rb.velocity = new Vector2(newXVel, newYVel);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        if (isDashing)
        {
            // only cancel momentum if the dash hasn't been canceled by something
            isDashing = false;
            float xCancel = inputDir.x * dashingPowerX;
            float yCancel = IsGrounded() ? 0 : inputDir.y * dashingPowerY;
            rb.velocity -= new Vector2(xCancel, yCancel);
        }
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
