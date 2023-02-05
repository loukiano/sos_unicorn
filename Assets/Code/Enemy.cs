using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public BoxCollider2D box;
	public SpriteRenderer spr;

	public Transform playerTransform;

	public float moveSpeed;
	public float health;
	public float maxHealth;
	public float atkDmg;
	public float bloodValue;




	// Use this for initialization
	void Start()
	{
		moveSpeed = 5;
		maxHealth = 100;
		health = maxHealth;
		atkDmg = 2;
		bloodValue = 100;


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
		ChasePlayer();

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
			return bloodValue;
        }
		return 0;
    }

	private void Die()
    {
		Destroy(gameObject);
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

