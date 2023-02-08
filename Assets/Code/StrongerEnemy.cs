using UnityEngine;
using System.Collections;

public class StrongerEnemy : MonoBehaviour
{
	public BoxCollider2D box;
	public SpriteRenderer spr;

	public Transform playerTransform;

	public float moveSpeed;
	public float health;
	public float maxHealth;
	public float atkDmg;
	public float bloodValue;

	public float deathDur;
	private float deathTime;

	private bool isInvincible;
	public float dashingTime = 0.2f;


	// Use this for initialization
	void Start()
	{
		moveSpeed = 5;
		maxHealth = 300;
		health = maxHealth;
		atkDmg = 20;
		bloodValue = 20;
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


    }

	// Update is called once per frame
	void Update()
	{
        if (health <= 0)
        {
            Die();
        }
		else
		{
			if (health <= 100)
			{
				spr.color = Color.red;
			}
			else if (health <= 200)
			{
				spr.color = Color.blue;
			}
			ChasePlayer();
		}

	}

	public float GetAtkDmg()
    {
		return atkDmg;
    }

	public IEnumerator TakeDamage(float dmg)
	{
		// Use your own damage handling code, or this example one.
		if (!isInvincible)
		{
			health -= dmg;
		}

		if (health <= 0)
		{
			// once dead, dont interact with player anymore;
			box.enabled = false;
            spr.color = Color.gray;
            deathTime = Time.time;
		}

		isInvincible = true;
		Debug.Log("isInvincible: " + isInvincible);
		yield return new WaitForSeconds(dashingTime);
		isInvincible = false;


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
			player.StrongerEnemyCollision(this);
        }
    }


}

