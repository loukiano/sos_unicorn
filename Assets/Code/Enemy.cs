using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public BoxCollider2D box;
	public SpriteRenderer spr;

	public Transform playerTransform;

	private TutorialTransition tutorialTransition;

	public float moveSpeed;
	public float health;
	public float maxHealth;
	public float atkDmg;
	public float bloodValue;

	public float deathDur;
	private float deathTime;

	// Use this for initialization
	void Start()
	{
		moveSpeed = 5;
		maxHealth = 100;
		health = maxHealth;
		atkDmg = 10;
		bloodValue = 7;
		deathDur = 0.25f;


        spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            spr = gameObject.AddComponent<SpriteRenderer>();
        }

        box = GetComponent<BoxCollider2D>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider2D>();
        }

		tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
    }

	// Update is called once per frame
	void Update()
	{
		if (health <= 0)
		{
			if (!(tutorialTransition.FinishedTutorialHuh()))
				tutorialTransition.SendMessage("StartGame");
			Die();
		}
		else
		{
			if (tutorialTransition.FinishedTutorialHuh())
				ChasePlayer();
		}

	}

	public float GetAtkDmg()
    {
		return atkDmg;
    }

	public float TakeDamage(float dmg)
		// returns amt of health stolen
	{
		health -= dmg;

		if (health <= 0)
        {
			// once dead, dont interact with player anymore;
			box.enabled = false;
			deathTime = Time.time;
			spr.color = Color.gray;
			return bloodValue;
        }
		return 0;
    }

	private void Die()
    {
		Color c = spr.color;
		c.a = (1 - (Time.time - deathTime)); // fade out over deathtime
		if (c.a <= 0)
        {
			Destroy(gameObject);
        } else
        {
			Debug.Log("fading from " + spr.color.a + " to " + c.a);
			spr.color = c;
        }
    }

	private void ChasePlayer()
    {
		Vector2 vel = (GetDirToPlayer() * moveSpeed) * Time.deltaTime;

        transform.position += new Vector3(vel.x, vel.y, transform.position.z);
    }

	private Vector2 GetDirToPlayer()
    {
		Vector2 dir = playerTransform.position - transform.position;
		dir.Normalize();
		return dir;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject collidingObject = collider.gameObject;
		Player player = collidingObject.GetComponent<Player>();
        if (player != null)
        {
            Debug.Log("HIT PLAYER");
			player.EnemyCollision(this);
        }
    }


}

