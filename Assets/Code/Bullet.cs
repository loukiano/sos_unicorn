using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 3;
    private Player p;

    private void Awake()
    {
        p = GetComponent<Player>();

        Destroy(gameObject, life);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        if (collidingObject.GetComponent<Enemy>() != null)
        {
            Debug.Log("shot em");
            Health enemyHealth = collidingObject.GetComponent<Health>();
            float bloodValue = enemyHealth.TakeDamage(100f);

        }

    }
}
