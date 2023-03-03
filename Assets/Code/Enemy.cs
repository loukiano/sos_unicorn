using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public SpriteRenderer spr;
    public GameObject bloodExplosion;
    public FlierSpriteHandler spriteHandler;

    private Health health;
    private Color enemyGreen;
    public int powerLevel;

    // Use this for initialization
    void Start()
	{

        spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            spr = gameObject.AddComponent<SpriteRenderer>();
        }
        health = GetComponent<Health>();

        enemyGreen = spr.color;


        if (health.health <= 100)
        {
            powerLevel = 1;
        }
        else if (health.health <= 200)
        {
            powerLevel = 2;
        }
        else
        {
            powerLevel = 3;
        }
    }

	// Update is called once per frame
	void Update()
	{
        if (health.isDead())
        {
            if (!World.isRunning)
                World.StartRunning();
            spr.color = Color.grey;
        }
        else if (health.health <= 100)
        {
            powerLevel = 1;
        }
        else if (health.health <= 200)
        {
            powerLevel = 2;
        }
        else
        {
            powerLevel = 3;
        }

	}

    public void OnDeath()
    {
        spr.color = Color.grey;
        Instantiate(bloodExplosion, transform.position, Quaternion.identity);
        //bloodExplosion.GetComponent<BloodSuck>().Explode();
    }

	


}

