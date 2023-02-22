using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UIElements;
using UnityEngine.SocialPlatforms.Impl;

public class Health : MonoBehaviour
{

    private BoxCollider2D box;
    private SpriteRenderer spr;
    public HealthBar healthBar;
    private Rigidbody2D rb;

	public float maxHealth;
	public float health;

    public float bloodValue;

	public bool isInvincible;
	public float invincibleDur;

    public float deathDur;
    public float deathTime;

    public bool isPlayer;
    public float bleedRate; // default 1/60f
    public float initialBleedRate;
    public float bleedTimeScaling;
    public float bleedScaleChunks;
    public float maxBleedRate;

    public bool doBleed;

	// Use this for initialization
	void Start()
	{
        health = maxHealth;
        deathTime = 0;
        box = gameObject.GetComponent<BoxCollider2D>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider2D>();
        }

        spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            spr = gameObject.AddComponent<SpriteRenderer>();
        }

        rb = GetComponent<Rigidbody2D>();
        if (isPlayer)
        {
            GameObject maybeHealthBar = GameObject.Find("HealthBar");
            if (maybeHealthBar != null)
            {
                healthBar = maybeHealthBar.GetComponent<HealthBar>();

            }
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            Die();
        }
        else if (deathTime > 0)
        {
            if (health > 0)
            {
                health -= 1 / 2f;
            }
            Die();
        }
        else if (isPlayer && doBleed && World.timer > 0 && World.isRunning)
        {
            if (bleedRate < maxBleedRate)
            {
                bleedRate = Mathf.Floor(World.timer / bleedScaleChunks) * bleedTimeScaling + initialBleedRate;
            }
            else
            {
                bleedRate = maxBleedRate;
            }
            health -= bleedRate;
            
        }

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar();
        }
    }

    public bool isDead()
    {
        return deathTime != 0;
    }


    public float TakeDamage(float dmg)
    {
        float bloodRet = 0;
        // Use your own damage handling code, or this example one.
        if (!isInvincible)
        {
            health -= dmg;
            bloodRet = bloodValue;
            if (healthBar != null)
            {
                healthBar.UpdateHealthBar();
            }
        }

        if (health <= 0)
        {
            // once dead, dont interact with anything anymore;
            gameObject.SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            transform.parent.SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);
            deathTime = Time.time;
        }
        StartCoroutine(TurnInvincible());
        return bloodRet;
    }

    public void HealDamage(float healAmt)
    {
        //Debug.Log("Healing " + healAmt);
        health += healAmt;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        if (healthBar != null)
        {
            healthBar.UpdateHealthBar();
        }

    }

    private IEnumerator TurnInvincible()
    {
        isInvincible = true;
        //Debug.Log("isInvincible: " + isInvincible);
        yield return new WaitForSeconds(invincibleDur);
        isInvincible = false;
    }

    public void Die()
    {
        Color c = spr.color;
        c.a = (deathDur - (Time.time - deathTime)); // fade out over deathDur
        if (c.a <= 0)
        {
            if (!isPlayer)
            {
                
                transform.parent.SendMessage("OnChildDestroy", SendMessageOptions.DontRequireReceiver);
                Destroy(gameObject);
            }
        }
        else
        {
            //Debug.Log("fading from " + spr.color.a + " to " + c.a);
            spr.color = c;
        }
    }
}

