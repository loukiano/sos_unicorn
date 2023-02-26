using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GroundedAI : AIController
{
    public Vector2 dirMove; // normalized vector pointing towards player
    public float detectionRadius;

    private Jumpable jump;

    private Transform playerTransform; // transform of player
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        playerTransform = GameObject.Find("Player").transform;
        dirMove = Random.value <= 0.5f ? Vector2.left : Vector2.right;

        jump = GetComponent<Jumpable>();

    }

    // Update is called once per frame
    public override void FixedUpdate()
    {
        if (World.isRunning)
        {
            DoTargeting();
            if (canMove)
            {
                DoMovement();
                MaybeJump();
            }
            if (jump.isJumping)
            {
                MaybeStopJump();
            }
        }
    }

    public override void DoMovement()
    {
        /* 
         * COOL BUT NOT WORTH THE EFFORT TO USE RN -- MAYBE LATER O.o
        // diff between move speed and how fast we're going now
        float velDif = moveSpeed - Vector2.Dot(rb.velocity, dirToPlayer);

        // how much are we accelerating
        float accelRate = (Mathf.Abs(moveSpeed) > 0.01f) ? acceleration : decceleration;

        // calculate movement force
        float moveForce = Mathf.Pow(Mathf.Abs(velDif) * accelRate, velPower) * Mathf.Sign(velDif);

        rb.AddForce(moveForce * dirToPlayer);
        */
        
        rb.velocity = moveSpeed * dirMove;
        
    }

    public override void DoTargeting()
    {
        Vector2 corner = ClosestFacingCorner();
        if (corner.Equals(Vector2.positiveInfinity))
        {
            canMove = false;
        } else
        {
            canMove = true;
        }
        if ((dirMove.x > 0 && box.bounds.max.x >= corner.x) ||
            (dirMove.x < 0 && box.bounds.min.x <= corner.x) ||
                // if we're hangin off the edge
            AmFacingWall())
                // if we're facing a wall
        {
            dirMove.x *= -1; // turn around
        }
    }

    public Vector2 ClosestFacingCorner()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        Collider2D ground = Physics2D.OverlapBox(box.bounds.center, new Vector2(box.bounds.size.x, box.bounds.size.y), 0, groundMask);
        if (ground != null)
        {
            if (dirMove.x < 0)
            {
                return new Vector2(ground.bounds.min.x, ground.bounds.max.y);
            }
            else
            {
                return new Vector2(ground.bounds.max.x, ground.bounds.max.y);
            }
        }
        return Vector2.positiveInfinity;
    }

    public bool AmFacingWall()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        RaycastHit2D lookAhead = Physics2D.Raycast(transform.position, dirMove, box.size.x+0.01f, groundMask);
        return lookAhead.collider != null;
    }

    public void MaybeJump()
    {
        LayerMask mask = LayerMask.GetMask("Default", "Ground");
        RaycastHit2D lookUp = Physics2D.Raycast(transform.position, Vector2.up, detectionRadius, mask);
        if (lookUp.collider != null)
        {
            //Debug.Log("Hit Something: " + lookUp.collider.ToString());
            Player player = lookUp.collider.gameObject.GetComponent<Player>();
            if (player != null)
                // we saw the player!
            {
                //Debug.Log("Hit Player!");
                if (playerTransform.position.y > transform.position.y + 1)
                {
                    canMove = false;
                    rb.velocity = Vector2.zero;
                    jump.DoJump();
                }
                
            } else
            {
                //Debug.Log("Hit " + lookUp.collider.gameObject.ToString());
            }
        }
    }

    public void MaybeStopJump()
    {
        if (jump.isJumping && playerTransform.position.y < transform.position.y)
        {
            jump.StopJump();
        }
    }

    private bool IsGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        return Physics2D.OverlapBox(box.bounds.center, new Vector2(box.bounds.size.x, box.bounds.size.y), 0, groundMask);
    }

    public void OnChildDestroy()
    {
        //Debug.Log("Child died");
        Destroy(gameObject);
    }

}
