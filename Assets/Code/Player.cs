using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller c;
    Rigidbody2D rb;
    SpriteRenderer spr;
    BoxCollider2D box;

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

    //kick variables
    public float kickCooldown;
    public float kickDuration;
    public float kickVel;
    public Vector2 kickSize;
    public bool isKicking;
    private bool canKick;


    public float health, maxHealth;
    public HealthBar healthBar;


    // Start is called before the first frame update
    void Start()
    {
        health = 100;
        maxHealth = 100;

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


    }

    // Update is called once per frame
    void Update()
    {
        if (isKicking)
        {
            spr.color = Color.red;
        }
        else if (isDashing)
        {
            spr.color = Color.magenta;
        }
        else if (IsGrounded())
        {
            spr.color = Color.green;
        } else
        {
            spr.color = Color.blue;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HealDamage();
            // TakeDamage();
        }

        health -= 1 / 30f;
        healthBar.UpdateHealthBar();
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            HandleMovement();
            ApplyFriction();
            HandleGravity();
        }
    }

    public void TakeDamage()
    {
        // Use your own damage handling code, or this example one.
        health = health - 5f;
        healthBar.UpdateHealthBar();
    }

    public void HealDamage()
    {
        health += 5f;
        Debug.Log(health);
        Debug.Log(maxHealth);
        healthBar.UpdateHealthBar();
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

        // on any (?) collision, refresh jumps
        if (collision.collider.bounds.center.y < transform.position.y && jumpsLeft < numJumps)
        {
            //Debug.Log("refreshing Jumps!");
            if (isDashing)
            {
                isDashing = false;
            }

            isJumping = false;
            jumpsLeft = numJumps;
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
