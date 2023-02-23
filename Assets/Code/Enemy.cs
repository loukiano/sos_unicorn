using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public SpriteRenderer spr;
    public ParticleSystem bloodExplosion;

    private Health health;

    private Color enemyGreen;

    // Use this for initialization
    void Start()
	{

        spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            spr = gameObject.AddComponent<SpriteRenderer>();
        }
        health = GetComponent<Health>();

        enemyGreen = new Color(46f / 255f, 173f / 255f, 94f / 255f);

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
            spr.color = enemyGreen;
        }
        else if (health.health <= 200)
        {
            spr.color = Color.blue;
        }
        else
        {
            spr.color = Color.yellow;
        }

	}

    public void OnDeath()
    {
        spr.color = Color.grey;
        Instantiate(bloodExplosion, transform.position, Quaternion.identity);
        //bloodExplosion.GetComponent<BloodSuck>().Explode();
    }

	


}

