using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life = 0.5f;
    private Player p;
    public bool hit = false;
    public Transform t;
    private Vector3 normalTransform;

    private void Start()
    {
        p = GetComponent<Player>();
        t = GetComponent<Transform>();
        normalTransform = t.localScale * 15;
    }
    private void Awake()
    {
        
        Destroy(gameObject, 1.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collidingObject = collision.gameObject;

        if (collidingObject.GetComponent<Enemy>() != null)
        {
            //Debug.Log("shot em");
            StartCoroutine(Expand());
            Health enemyHealth = collidingObject.GetComponent<Health>();
            float bloodValue = enemyHealth.TakeDamage(100f);
            Health playerHealth = FindObjectOfType<Player>().GetComponent<Health>();
            playerHealth.HealDamage(100f);
            ScoreUI scoreUI = FindObjectOfType<ScoreUI>();
            scoreUI.Score();
        } 

    }

    private IEnumerator Expand()
    {
        t.localScale = normalTransform;
        yield return new WaitForSeconds(0.05f);
        Destroy(gameObject);

    }
}
