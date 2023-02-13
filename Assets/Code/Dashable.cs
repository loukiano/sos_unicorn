using UnityEngine;
using System.Collections;

public class Dashable : MonoBehaviour
{

    // IN PROGRESS: SEPARATING OUT ALL ACTIONS INTO COMPONENTS

    Rigidbody2D rb;
    BoxCollider2D box;

    // dash variables
    public float dashVel = 40;
    public bool isDashing;
    public float dashingTime = 0.1f;
    public float dashingCooldown = 2.5f;
    public float dashDmg = 100;
    public bool canDash = true;

    // Use this for initialization
    void Start()
	{
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        box = GetComponent<BoxCollider2D>();
    }

	// Update is called once per frame
	void Update()
	{
			
	}

    public void DoDash(Vector2 dir)
    {
        if (canDash)
        {
            StartCoroutine(Dash(dir));
        }
    }

    private IEnumerator Dash(Vector2 dir)
    {
        if (dir.magnitude == 0)
        // neutral dash
        {
            dir = new Vector2(1, 0);
        } else if (dir.magnitude != 1)
        {
            dir.Normalize();
        }
        canDash = false;
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

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

        rb.velocity = new Vector2(newXVel, newYVel);

        // TODO: handle this in update loop instead of being a coroutine
        yield return new WaitForSeconds(dashingTime);
        rb.gravityScale = originalGravity;
        if (isDashing)
        {
            // only cancel momentum if the dash hasn't been canceled by something
            isDashing = false;
            float xCancel = dir.x * dashVel;
            float yCancel = IsGrounded() ? 0 : dir.y * dashVel;
            rb.velocity -= new Vector2(xCancel, yCancel);
        }
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

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

