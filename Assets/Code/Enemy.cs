using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public SpriteRenderer spr;

    private Health health;
	TutorialTransition tutorialTransition;

    private Color enemyRed;

    // Use this for initialization
    void Start()
	{

        spr = GetComponent<SpriteRenderer>();
        if (spr == null)
        {
            spr = gameObject.AddComponent<SpriteRenderer>();
        }

		tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
        health = GetComponent<Health>();

        enemyRed = new Color(1f, 13f / 255f, 0f); ;

    }

	// Update is called once per frame
	void Update()
	{
        if (health.isDead())
        {
            if (!(tutorialTransition.FinishedTutorialHuh()))
                tutorialTransition.SendMessage("StartGame");
            spr.color = Color.grey;
        }
        else if (health.health <= 100)
        {
            spr.color = enemyRed;
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

	


}

