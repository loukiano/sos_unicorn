using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public Enemy enemy;
	public BoxCollider2D mapBounds;
	public LevelDesign levelDesign;
	public Camera camera;
	public float spawnCheck;

	private float timer;
	private float xMin, xMax, yMin, yMax;
	private float camOrthsize;
	private float cameraRatio;

	// Use this for initialization
	void Start()
	{
		timer = 0.0f;
		xMin = mapBounds.bounds.min.x;
		xMax = mapBounds.bounds.max.x;
		yMin = mapBounds.bounds.min.y;
		yMax = mapBounds.bounds.max.y;
		camOrthsize = camera.orthographicSize;
		cameraRatio = (xMax + camOrthsize) / 2.0f;
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= spawnCheck)
        {
			timer -= spawnCheck;
			Debug.Log("Spawning...");
			SpawnEnemy();
        }
	}


	// Randomly spawns enemies within bounds.
	void SpawnEnemy()
	{
		float xPosition = Random.value * (xMax - xMin) + xMin;
		float yPosition = Random.value * (yMax - yMin) + yMin;
		Instantiate(enemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
    }

	// Randomly spawns enemies within bounds, but outside of the player's view. Higher
	// probability of enemies spawning just outside of the player's view.
	void SpawnEnemyOutOfCamera()
    {
		// Constraints:
		// 1. Enemies can't spawn in the player's view
		// // a. Must be outside of the range of (yCam + camOrthsize, yCam - camOrthsize) and (xMin + cameraRatio, xMax - cameraRatio)
		// 2. Enemies must spawn on a platform (preferably the one the player is currently standing on.)
		// ^^ Why? Because we want some amount of constant pressure. However! If the platform is too small, this allows for
		// some abuse from the player. So maybe we have two different kinds of spawners-- one that spawns on the player's current
		// platform, and one that populates around it.

		//float xPosition = Random.value * (xMax - xMin) + xMin;
		//float yPosition = Random.value * (yMax - yMin) + yMin;

		float xPosition = Random.value * 2.0f;
		float yPosition = Random.value * 2.0f;
		//camera = GetComponent<Camera>;

		// Spawn left of player
		if (xPosition <= 1.0f)
        {
			xPosition = Random.value * (camera.transform.position.x - cameraRatio - xMin) + xMin;
        }
		else
		{
			xPosition = Random.value * (xMax - (camera.transform.position.x + cameraRatio)) + camera.transform.position.x + cameraRatio;
		}
		if (yPosition <= 1.0f)
		{
			yPosition = Random.value * (camera.transform.position.y - camOrthsize - yMin) + yMin;
		}
		else
		{
			yPosition = Random.value * (yMax - (camera.transform.position.y + camOrthsize)) + camera.transform.position.y + camOrthsize;
		}

		/*
		float x = 0;
		foreach (Transform child in levelDesign.transform)
        {
			Debug.Log("Ground " + x);
			x++;
        } */

		Instantiate(enemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
	}
}

