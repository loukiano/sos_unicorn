using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public SpriteRenderer spr;

    private Health health;
	TutorialTransition tutorialTransition;

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
            spr.color = Color.red;
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

