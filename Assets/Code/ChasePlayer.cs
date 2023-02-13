using UnityEngine;
using System.Collections;

public class ChasePlayer : AIController
{
	public Vector2 dirToPlayer; // normalized vector pointing towards player

	private Transform playerTransform; // transform of player
    TutorialTransition tutorialTransition;
    // Use this for initialization
    public override void Start()
	{
		base.Start();
		playerTransform = GameObject.Find("Player").transform;
        tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
        canMove = tutorialTransition.FinishedTutorialHuh();
    }

	// Update is called once per frame
	public override void FixedUpdate()
	{
        canMove = tutorialTransition.FinishedTutorialHuh();
        if (canMove)
        {
			DoTargeting();
			DoMovement();
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

        rb.velocity = moveSpeed * dirToPlayer;
        
    }

    public override void DoTargeting()
    {
        dirToPlayer = playerTransform.position - transform.position;
        dirToPlayer.Normalize();
    }


}

