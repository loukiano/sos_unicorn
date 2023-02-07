using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public Enemy enemy;
	public StrongEnemy strongEnemy;
	public StrongerEnemy strongerEnemy;
	public BoxCollider2D mapBounds;
	public Transform playerTransform;
	//public LevelDesign levelDesign;
	public Camera camera;
	public float spawnCheck;
	public int initialNumEnemies = 20;

	private float timer;
	private float xMin, xMax, yMin, yMax;
	private float camOrthsize;
	private float cameraRatio;
	private float enemySizeX;
	private float enemySizeY;

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
		BoxCollider2D enemyBounds = enemy.GetComponent<BoxCollider2D>();
		enemySizeX = enemyBounds.bounds.max.x - enemyBounds.bounds.center.x;
		enemySizeY = enemyBounds.bounds.max.y - enemyBounds.bounds.center.y;

		playerTransform = GameObject.Find("Player").transform;


		for (int i = 0; i < initialNumEnemies; i++)
        {
			SpawnEnemyOutOfCamera();
        }
	}

	// Update is called once per frame
	void Update()
	{
		timer += Time.deltaTime;
		if (timer >= spawnCheck)
        {
			timer -= spawnCheck;
			Debug.Log("Spawning...");
			SpawnEnemyOutOfCamera();
        }
	}


	// Randomly spawns enemies within bounds.
	void SpawnEnemy()
	{
		float xPosition = Random.value * (xMax - xMin) + xMin;
		float yPosition = Random.value * (yMax - yMin) + yMin;

		int enemyProbability = Random.Range(0, 10);
		
		if (enemyProbability < 5)
		{
			Instantiate(enemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
		}
		else if (enemyProbability < 8)
        {
			Instantiate(strongEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
		}
		else
        {
			Instantiate(strongerEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
		}
	}

	// Randomly spawns enemies within bounds, but outside of the player's view. Higher
	// probability of enemies spawning just outside of the player's view.
	// TASKS TO DO:
	// Make sure that we're accounting for the size of the enemy (not just the center outside of the camera range, but the full enemy.
	void SpawnEnemyOutOfCamera()
    {
		// Constraints:
		// 1. Enemies can't spawn in the player's view
		// // a. Must be outside of the range of (yCam + camOrthsize, yCam - camOrthsize) and (xMin + cameraRatio, xMax - cameraRatio)

		float xOrY = Random.value;
		float xPosition = Random.value * 2.0f;
		float yPosition = Random.value * 2.0f;

		if (xOrY < 0.5f) {
			// separate on x axis
			// Spawn left of player
			if (xPosition <= 1.0f)
			{
				xPosition = Random.value * (camera.transform.position.x - cameraRatio - xMin) + xMin - enemySizeX;
			}
			// Spawn right of player
			else
			{
				xPosition = Random.value * (xMax - (camera.transform.position.x + cameraRatio)) + camera.transform.position.x + cameraRatio + enemySizeX;
			}

			yPosition = Random.value * (yMax - yMin) + yMin;
		}
		else
        {
			// separate on y axis
			// Spawn above player
			if (yPosition <= 1.0f)
			{
				yPosition = Random.value * (camera.transform.position.y - camOrthsize - yMin) + yMin - enemySizeY;
			}
			// Spawn below player
			else
			{
				yPosition = Random.value * (yMax - (camera.transform.position.y + camOrthsize)) + camera.transform.position.y + camOrthsize - enemySizeY;
			}

			xPosition = Random.value * (xMax - xMin) + xMin;
		}

		int enemyProbability = Random.Range(0,10);

		if (enemyProbability < 5)
        {
			Enemy newEnemy = Instantiate(enemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
			newEnemy.playerTransform = playerTransform;
		} else if (enemyProbability < 8)
        {
			StrongEnemy newStrongEnemy = Instantiate(strongEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
			newStrongEnemy.playerTransform = playerTransform;
		} else
        {
			StrongerEnemy newStrongerEnemy = Instantiate(strongerEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
			newStrongerEnemy.playerTransform = playerTransform;
		}
		
	}

	// Same constraints as out of camera view, with the additional constraint that enemies must spawn on a platform.
	// NEED TO IMPLEMENT
	void SpawnEnemyOnPlatform()
    {
		
    }
}

