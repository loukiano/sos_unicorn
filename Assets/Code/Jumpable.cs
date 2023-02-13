using UnityEngine;
using System.Collections;

public class Jumpable : MonoBehaviour
{
    private Rigidbody2D rb;
    private Dashable dash;


    // jumping stuff
    public int numJumps = 2;
    private int jumpsLeft = 2;
    private float lastJump = 0; // when was the player's last jump
    private bool isJumping = false;
    public float jumpCooldown = 0f; // seconds
    public float jumpForce = 25;
    public float jumpStopMultiplier = 0.5f;


    // Use this for initialization
    void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        dash = GetComponent<Dashable>();
    }

	// Update is called once per frame
	void Update()
	{
			
	}

    // called via messages by the Controller
    public void DoJump()
    {
        if (Time.time > lastJump + jumpCooldown && jumpsLeft > 0)
        {
            if (dash.isDashing)
            {
                dash.StopDash();
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

    // called via messages by the Controller
    public void StopJump()
    {
        if (rb.velocity.y > 0 && isJumping)
        {
            //Debug.Log("Stopping jump!");
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpStopMultiplier), ForceMode2D.Impulse);

        }
    }

    public void RefreshJumps()
    {
        isJumping = false;
        jumpsLeft = numJumps;
    }
}

