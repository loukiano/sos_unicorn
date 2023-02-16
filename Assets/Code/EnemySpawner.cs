using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemy;
	public GameObject strongEnemy;
	public GameObject strongerEnemy;

	public BoxCollider2D mapBounds;
	public Transform playerTransform;
	//public LevelDesign levelDesign;

	public Camera cam;
	public float spawnRate;
	public float initialSpawnRate;
    public float spawnrateTimeScaling;
	public float spawnrateScaleChunks;
	public float maxSpawnrate;

    public int initialNumEnemies;
	public float timeSpawnStrong = 5;
	public float timeSpawnStronger = 10;

	private float lastSpawn;
	private float xMin, xMax, yMin, yMax;
	private float camOrthsize;
	private float camRatio;
	private float enemySizeX;
	private float enemySizeY;
	private TutorialTransition tutorialTransition;

	private int normalProb = 5;
	private int strongProb = 8;
	private int strongerProb = 10;

	

	// Use this for initialization
	void Start()
	{
		xMin = mapBounds.bounds.min.x;
		xMax = mapBounds.bounds.max.x;
		yMin = mapBounds.bounds.min.y;
		yMax = mapBounds.bounds.max.y;
		camOrthsize = cam.orthographicSize;
		camRatio = (xMax + camOrthsize) / 2.0f;
		BoxCollider2D enemyBounds = enemy.GetComponent<BoxCollider2D>();
		enemySizeX = enemyBounds.bounds.max.x - enemyBounds.bounds.center.x;
		enemySizeY = enemyBounds.bounds.max.y - enemyBounds.bounds.center.y;

		playerTransform = GameObject.Find("Player").transform;

        for (int i = 0; i < initialNumEnemies; i++)
        {
			SpawnEnemyOutOfCamera();
        }
		tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
		//spawnRate = initialSpawnRate;
	}

	// Update is called once per frame
	void Update()
	{
		if (tutorialTransition.FinishedTutorialHuh())
		{

			if (spawnRate > maxSpawnrate)
            {
				spawnRate = (1f / (Mathf.Floor(World.timer / spawnrateScaleChunks) * spawnrateTimeScaling + initialSpawnRate));
				Debug.Log("step: " + (Mathf.Floor(World.timer / spawnrateScaleChunks)));
            } else
            {
				spawnRate = maxSpawnrate;
            }


			if (World.timer >= lastSpawn + spawnRate)
			{
				Debug.Log("Spawning...");
				SpawnEnemyOutOfCamera();
				lastSpawn = World.timer;
			}
		} else
        {
			//Debug.Log("Timer: " + Time.deltaTime);
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
	// Make sure that we're accounting for the size of the enemy (not just the center outside of the cam range, but the full enemy.
	void SpawnEnemyOutOfCamera()
    {
		// Constraints:
		// 1. Enemies can't spawn in the player's view
		// // a. Must be outside of the range of (yCam + camOrthsize, yCam - camOrthsize) and (xMin + camRatio, xMax - camRatio)

		float xOrY = Random.value;
		float xPosition = Random.value * 2.0f;
		float yPosition = Random.value * 2.0f;

		if (xOrY < 0.5f) {
			// separate on x axis
			// Spawn left of player
			if (xPosition <= 1.0f)
			{
				xPosition = Random.value * (cam.transform.position.x - camRatio - xMin) + xMin - enemySizeX;
			}
			// Spawn right of player
			else
			{
				xPosition = Random.value * (xMax - (cam.transform.position.x + camRatio)) + cam.transform.position.x + camRatio + enemySizeX;
			}

			yPosition = Random.value * (yMax - yMin) + yMin;
		}
		else
        {
			// separate on y axis
			// Spawn above player
			if (yPosition <= 1.0f)
			{
				yPosition = Random.value * (cam.transform.position.y - camOrthsize - yMin) + yMin - enemySizeY;
			}
			// Spawn below player
			else
			{
				yPosition = Random.value * (yMax - (cam.transform.position.y + camOrthsize)) + cam.transform.position.y + camOrthsize - enemySizeY;
			}

			xPosition = Random.value * (xMax - xMin) + xMin;
		}

		int maxProb = normalProb;
		if (Time.time > timeSpawnStrong)
        {
			maxProb = (Time.time < timeSpawnStronger) ? strongProb : strongerProb;
        }


		int enemyProbability = Random.Range(0, maxProb);
		GameObject newEnemy;
		if (enemyProbability < 5)
        {
			newEnemy = Instantiate(enemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
		} else if (enemyProbability < 8)
        {
            newEnemy = Instantiate(strongEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
		} else
        {
            newEnemy = Instantiate(strongerEnemy, new Vector3(xPosition, yPosition, 0), Quaternion.identity);
		}
		
	}

	// Same constraints as out of cam view, with the additional constraint that enemies must spawn on a platform.
	// NEED TO IMPLEMENT
	void SpawnEnemyOnPlatform()
    {
		
    }
}

