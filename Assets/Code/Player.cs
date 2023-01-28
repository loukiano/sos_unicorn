using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller c;
    Rigidbody2D rb;
    SpriteRenderer spr;

    public float castDist = 1;

    public float walkSpeed = 7;
    public float walkAccel = 0.1f; // how quick the player gets up to speed

    public int numJumps = 2;
    private int jumpsLeft = 2;
    private float lastJump = 0; // when was the player's last jump
    public float jumpCooldown = 0.5f; // seconds
    public Vector2 jumpForce = new Vector2(0, 10);

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


    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing) {
            return;
        }
        HandleMovement();

        if (IsGrounded())
        {
            spr.color = Color.green;
        } else
        {
            spr.color = Color.blue;
        }
    }

    public void HandleMovement()
    {
        Vector2 inputDir = c.GetInputDir();
        float xvel = Mathf.Lerp(rb.velocity.x, inputDir.x * walkSpeed, walkAccel);
        float yvel = rb.velocity.y;

        rb.velocity = new Vector2(xvel, yvel);

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void DoJump()
    {
        if (Time.time > lastJump + jumpCooldown && jumpsLeft > 0)
        {
            Debug.Log("Jumping!");
            lastJump = Time.time;
            jumpsLeft -= 1;
            rb.AddForce(jumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
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
        return Physics2D.Raycast(gameObject.transform.position, Vector2.down, castDist, groundMask);
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
