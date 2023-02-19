using UnityEngine;
using System.Collections;

public class AIController : MonoBehaviour
{

	public Rigidbody2D rb;
	public BoxCollider2D box;
	public SpriteRenderer spr;

	public bool canMove;
	public float moveSpeed;

    public virtual void Start()
    {

        rb = gameObject.GetComponent<Rigidbody2D>();
		if (rb == null)
		{
			rb = gameObject.AddComponent<Rigidbody2D>();
        }

		box = gameObject.GetComponent<BoxCollider2D>();
		if (box == null)
        {
			box = gameObject.AddComponent<BoxCollider2D>();
        }

		spr = gameObject.GetComponent<SpriteRenderer>();
    }

	public virtual void FixedUpdate()
    {
		if (canMove)
        {
			DoTargeting();
			DoMovement();
        }
    }

	public virtual void Freeze()
    {
		canMove = false;
		rb.simulated = false;
    }

	public virtual void UnFreeze()
    {
		canMove = true;
		rb.simulated = true;
    }


	public virtual void DoMovement() { }

	public virtual void DoTargeting() { }


}

