using UnityEngine;
using System.Collections;

public class Dashable : MonoBehaviour
{

    // IN PROGRESS: SEPARATING OUT ALL ACTIONS INTO COMPONENTS

    Rigidbody2D rb;
    BoxCollider2D box;
    Transform t;
    Kickable kick;

    // Gravity
    public float originalGravity;

    // dash variables
    public float dashVel;
    public bool isDashing;
    public float dashingTime;
    public int maxDashes = 1;
    public int numDashes;
    public float dashDmg;
    public float dashSize;
    public bool inCoyoteTime;
    public float coyoteTime;

    public Vector2 aimDir;

    private float lastDashTime;
    private float lastDashStopTime;
    private Vector2 cancelVel; // velocity to cancel the most recent dash
    private Vector3 normalTransformScale;
    //public Vector2 nextDash;

    // Use this for initialization
    void Start()
	{
        aimDir = Vector2.right;
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        originalGravity = rb.gravityScale;
        box = GetComponent<BoxCollider2D>();
        t = GetComponent<Transform>();
        normalTransformScale = t.localScale;
        kick = GetComponent<Kickable>();
    }

	// Update is called once per frame
	void Update()
	{
        if (lastDashTime + dashingTime < Time.time)
        {
            rb.gravityScale = originalGravity;
            if (isDashing)
            {
                // only cancel momentum if the dash hasn't been canceled by something
                isDashing = false;
                lastDashStopTime = Time.time;
                inCoyoteTime = true;
                if (IsGrounded())
                {
                    cancelVel *= Vector2.right;
                }
                rb.velocity -= cancelVel;
                if (!kick.isKicking)
                    t.localScale = normalTransformScale;
            }
        }
        if (lastDashStopTime + coyoteTime < Time.time && inCoyoteTime)
        {
            inCoyoteTime = false;
        }
        if (lastDashTime + dashingTime * 3 < Time.time && IsGrounded())
        {
            RefreshDashes();
        }
        
	}

    public void DoDash(Vector2 dir)
    {
        if (CanDash())
        {
            numDashes -= 1;
            
            if (dir.magnitude == 0)
            // neutral dash
            {
                dir = aimDir;
            }
            if (dir.magnitude != 1)
            {
                dir.Normalize();
            }


            rb.gravityScale = 0;

            float newXVel = dir.x * dashVel;
            if (Mathf.Sign(dir.x) == Mathf.Sign(rb.velocity.x))
            // conserve momentum if same direction
            {
                newXVel += rb.velocity.x;
            }

            float newYVel = dir.y * dashVel;
            if (Mathf.Sign(dir.y) == Mathf.Sign(rb.velocity.y))
            {
                newYVel += rb.velocity.y;
            }

            lastDashTime = Time.time; // marks the new dash
            isDashing = true;
            rb.velocity = new Vector2(newXVel, newYVel);

            t.localScale = normalTransformScale;
            t.localScale *= dashSize;
            kick.isKicking = false;

            gameObject.SendMessage("StartDash"); // allows for action on dash start

            float xCancel = dir.x * dashVel;
            float yCancel = dir.y * dashVel;
            cancelVel = new Vector2(xCancel, yCancel);

        }
    }

    public void AddDash()
    {
        numDashes += 1;
        if (numDashes > maxDashes)
        {
            numDashes = maxDashes;
        }
    }

    public bool CanDash()
    {
        return numDashes > 0;
    }

    public void RefreshDashes()
    {
        numDashes = maxDashes;
    }

    public void StopDash()
    {
        isDashing = false;
        if (!kick.isKicking)
        {
            t.localScale = normalTransformScale;
        }
    }

    public void AdjustVelocity()
    {
        rb.velocity -= new Vector2(cancelVel.x / 2, cancelVel.y / 3);
    }

    private bool IsGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        return Physics2D.OverlapBox(box.bounds.center, new Vector2(box.bounds.size.x, box.bounds.size.y), 0, groundMask);
    }
}

