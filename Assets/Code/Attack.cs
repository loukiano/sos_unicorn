using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    public BoxCollider2D box;

    public float atkDmg;
    public float atkRange; // 0.0 for melee, otherwise ranged
    public float atkSpeed;

    public enum Team {Enemy, Player};
    public Team team;

    
    private float lastAtk;

	// Use this for initialization
	void Start()
	{
        box = gameObject.GetComponent<BoxCollider2D>();
        if (box == null)
        {
            box = gameObject.AddComponent<BoxCollider2D>();
        }
        box.isTrigger = true;
    }

    private bool canAtk()
    {
        return Time.time > lastAtk + atkSpeed;
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        GameObject collidingObject = collider.gameObject;
        if (team == Team.Enemy && canAtk())
        {
            Player player = collidingObject.GetComponent<Player>();
            if (player != null)
            {
                Health playerHealth = collidingObject.GetComponent<Health>();
                //Debug.Log("HIT PLAYER");
                playerHealth.TakeDamage(atkDmg);
                lastAtk = Time.time;
            }
        }
        
    }
}

