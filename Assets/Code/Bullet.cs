using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 1;
    private Player p;
    public bool hit = false;

    private void Start()
    {
        p = GetComponent<Player>();
    }
    private void Awake()
    {
        
        Destroy(gameObject, life);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        if (collidingObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("shot em");
            Health enemyHealth = collidingObject.GetComponent<Health>();
            Health playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
            float bloodValue = enemyHealth.TakeDamage(100f);
            playerHealth.HealDamage(bloodValue);
            ScoreUI scoreUI = FindObjectOfType<ScoreUI>();
            scoreUI.Score();
        }

    }

    
}
