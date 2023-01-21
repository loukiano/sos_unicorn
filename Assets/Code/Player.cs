using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Controller c;
    Rigidbody2D rb;

    public float walkSpeed = 7;
    public float walkAccel = 0.1f; // how quick the player gets up to speed

    public int numJumps = 2;
    private int jumpsLeft = 2;
    private float lastJump = 0; // when was the player's last jump
    public float jumpCooldown = 0.5f; // seconds
    public Vector2 jumpForce = new Vector2(0, 10);

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
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
    }

    public void HandleMovement()
    {
        Vector2 inputDir = c.GetInputDir();
        float xvel = Mathf.Lerp(rb.velocity.x, inputDir.x * walkSpeed, walkAccel);
        float yvel = rb.velocity.y;

        rb.velocity = new Vector2(xvel, yvel);
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

}
