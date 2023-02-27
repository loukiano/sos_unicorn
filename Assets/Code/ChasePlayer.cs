using UnityEngine;
using System.Collections;

public class ChasePlayer : AIController
{
	public Vector2 dirMove; // normalized vector pointing towards player
    public float detectionRadius;
    private bool hasAggro;

	private Transform playerTransform; // transform of player
    private EnemySpawner spawner;
    // Use this for initialization
    public override void Start()
	{
		base.Start();
		playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        spawner = GetComponentInParent<EnemySpawner>();
        hasAggro = !spawner.isClearableArea;
        
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
        else
        {
            rb.velocity = 0f * dirMove;
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
        if (spawnArea != null && !hasAggro)
            // check if player has entered the area
        {
            //Debug.Log(spawnArea.bounds.ToString());
            hasAggro = spawner.spawnArea.bounds.Contains(new Vector2(playerTransform.position.x, playerTransform.position.y));
        } else
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


}

