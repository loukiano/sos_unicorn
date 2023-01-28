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
    private bool canDash = true;
    private bool isDashing;
    private float dashingPowerX = 24f;
    private float dashingPowerY = 12f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.5f;

    

    // Start is called before the first frame update
    void Start()
    {
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
        if (IsGrounded())
        {
            spr.color = Color.green;
        } else
        {
            spr.color = Color.blue;
        }

        if (isDashing) {
            return;
        }
    }

    void FixedUpdate()
    {
        HandleMovement();
        ApplyFriction();
        HandleGravity();
    }

    public void HandleMovement()
    {
        Vector2 inputDir = c.GetInputDir();

            // how fast do we want to be going
        float goalSpeed = inputDir.x * walkSpeed; 
            // diff between that and how fast we're going now
        float velDif = goalSpeed - rb.velocity.x; 
            // how much are we accelerating
        float accelRate = (Mathf.Abs(goalSpeed) > 0.01f) ? acceleration : decceleration;
            // calculate movement force
        float moveForce = Mathf.Pow(Mathf.Abs(velDif) * accelRate, velPower) * Mathf.Sign(velDif);

        rb.AddForce(moveForce * Vector2.right);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }


    }

    public void ApplyFriction()
    {
        if (IsGrounded() && Mathf.Abs(c.GetInputDir().x) < c.deadzone)
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
            Debug.Log("Jumping!");
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

    // called via messages by the Controller
    public void StopJump()
    {
        if (rb.velocity.y > 0 && isJumping)
        {
            Debug.Log("Stopping jump!");
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpStopMultiplier), ForceMode2D.Impulse);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isJumping)
        {
            isJumping = false; // we've landed
        }

        // on any (?) collision, refresh jumps
        if (jumpsLeft < numJumps)
        {
            Debug.Log("refreshing Jumps!");
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
        rb.velocity = new Vector2(inputDir.x * dashingPowerX, inputDir.y * dashingPowerY);
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

}
