using UnityEngine;
using System.Collections;

public class Dashable : MonoBehaviour
{

    // IN PROGRESS: SEPARATING OUT ALL ACTIONS INTO COMPONENTS

    Rigidbody2D rb;
    BoxCollider2D box;


    // Gravity
    public float originalGravity;

    // dash variables
    public float dashVel;
    public bool isDashing;
    public float dashingTime;
    public float dashingCooldown;
    public float dashDmg;
    public bool canDash;

    private float lastDashTime;
    private Vector2 cancelVel; // velocity to cancel the most recent dash
    //public Vector2 nextDash;

    // Use this for initialization
    void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        originalGravity = rb.gravityScale;
        box = GetComponent<BoxCollider2D>();

    }

	// Update is called once per frame
	void Update()
	{
        if (!canDash && lastDashTime + dashingCooldown < Time.time)
        {
            canDash = true;
        }
        if (lastDashTime + dashingTime < Time.time)
        {
            rb.gravityScale = originalGravity;
            if (isDashing)
            {
                // only cancel momentum if the dash hasn't been canceled by something
                isDashing = false;
                if (IsGrounded())
                {
                    cancelVel *= Vector2.right;
                }
                rb.velocity -= cancelVel;
            }
        }
	}

    public void DoDash(Vector2 dir)
    {
        if (canDash)
        {
            canDash = false;
            

            if (dir.magnitude == 0)
            // neutral dash
            {
                dir = new Vector2(1, 0);
            }
            else if (dir.magnitude != 1)
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

            float xCancel = dir.x * dashVel;
            float yCancel = dir.y * dashVel;
            cancelVel = new Vector2(xCancel, yCancel);


        }
    }

    //private IEnumerator Dash(Vector2 dir)
    //{
    //    if (dir.magnitude == 0)
    //    // neutral dash
    //    {
    //        dir = new Vector2(1, 0);
    //    } else if (dir.magnitude != 1)
    //    {
    //        dir.Normalize();
    //    }
    //    canDash = false;
    //    isDashing = true;

    //    float originalGravity = rb.gravityScale;
    //    rb.gravityScale = 0f;

    //    float newXVel = dir.x * dashVel;
    //    if (Mathf.Sign(dir.x) == Mathf.Sign(rb.velocity.x))
    //    // conserve momentum if same direction
    //    {
    //        newXVel += rb.velocity.x;
    //    }

    //    float newYVel = dir.y * dashVel;
    //    if (Mathf.Sign(dir.y) == Mathf.Sign(rb.velocity.y))
    //    {
    //        newYVel += rb.velocity.y;
    //    }

    //    rb.velocity = new Vector2(newXVel, newYVel);

    //    // TODO: handle this in update loop instead of being a coroutine
    //    yield return new WaitForSeconds(dashingTime);
    //    rb.gravityScale = originalGravity;
    //    if (isDashing)
    //    {
    //        // only cancel momentum if the dash hasn't been canceled by something
    //        isDashing = false;
    //        float xCancel = dir.x * dashVel;
    //        float yCancel = IsGrounded() ? 0 : dir.y * dashVel;
    //        rb.velocity -= new Vector2(xCancel, yCancel);
    //    }
    //    yield return new WaitForSeconds(dashingCooldown);
    //    canDash = true;
    //}

    public void StopDash()
    {
        isDashing = false;
    }

    private bool IsGrounded()
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        return Physics2D.OverlapBox(box.bounds.center, new Vector2(box.bounds.size.x, box.bounds.size.y), 0, groundMask);
    }
}

