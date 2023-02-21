using UnityEngine;
using System.Collections;

public class ChasePlayer : AIController
{
	public Vector2 dirMove; // normalized vector pointing towards player
    public float detectionRadius;

	private Transform playerTransform; // transform of player
    private EnemySpawner spawner;
    // Use this for initialization
    public override void Start()
	{
		base.Start();
		playerTransform = GameObject.Find("Player").transform;
        spawner = GetComponentInParent<EnemySpawner>();
        
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
        dirMove = playerTransform.position - transform.position;
        if (!spawner.spawnArea.bounds.Contains(playerTransform.position) && dirMove.magnitude > detectionRadius)
            // player is far away
        {
            dirMove = Vector2.zero;
        }
        dirMove.Normalize();
    }


}

