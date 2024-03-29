﻿using UnityEngine;
using System.Collections;

public class Jumpable : MonoBehaviour
{
    private Rigidbody2D rb;
    private Dashable dash;


    // jumping stuff
    public int numJumps = 2;
    private int jumpsLeft = 2;
    private float lastJump = 0; // when was the player's last jump
    public bool isJumping = false;
    public float jumpCooldown = 0f; // seconds
    public float jumpForce = 25;
    public float jumpStopMultiplier = 0.5f;
    public bool canJump = true;

    // Use this for initialization
    void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        dash = GetComponent<Dashable>();
        canJump = true;
    }

	// Update is called once per frame
	void Update()
	{
	    if (jumpsLeft == 0)
        {
            canJump = false;
        } else
        {
            canJump = true;
        }		
	}

    // called via messages by the Controller
    public void DoJump()
    {
        if (Time.time > lastJump + jumpCooldown && jumpsLeft > 0)
        {
            //Debug.Log("Jumping!");
            lastJump = Time.time;
            jumpsLeft -= 1;
            isJumping = true;
            PlayJumpSound();
            if (dash != null && dash.isDashing)
            {
                dash.StopDash();
                dash.AdjustVelocity();
                dash.RefreshDashes();
                //rb.AddForce(Vector2.up * jumpForce / 2, ForceMode2D.Impulse);
            }
            else
            {
                if (dash != null && dash.inCoyoteTime)
                {
                    dash.RefreshDashes();
                }
                if (rb.velocity.y < 0)
                // if falling
                {
                    rb.AddForce(Vector2.down * rb.velocity.y, ForceMode2D.Impulse);
                }

                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            }
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
        canJump = true;
    }

    private void PlayJumpSound()
    {
        float volume = 1;
        if (gameObject.tag == "Enemy")
        {
            Debug.Log("Enemyjump!");
            volume = 0.5f;
        }
        SoundPlayer.PlaySound(SoundPlayer.Sounds.jump, volume);
    }
}

