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
		bloodValue = 10;


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
		ChasePlayer();
	}

	public float GetAtkDmg()
    {
		return atkDmg;
    }

	public void TakeDamage(float dmg)
    {
		health -= dmg;

		if (health <= 0)
        {
			Die();
        }
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


}

