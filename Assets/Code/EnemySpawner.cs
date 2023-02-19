using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemy;
	public GameObject strongEnemy;
	public GameObject strongerEnemy;

	private BoxCollider2D spawnArea;
	//public LevelDesign levelDesign;

	public Camera cam;
	public float spawnRate; // interval betweeen enemy spawns
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

	public bool spawnOnGround;
	public float groundedSpawnOffset;


	private int normalProb = 5;
	private int strongProb = 8;
	private int strongerProb = 10;

	

	// Use this for initialization
	void Start()
	{

		if (spawnArea == null)
        {
			spawnArea = GetComponent<BoxCollider2D>();
			if (spawnArea == null)
            {
				spawnArea = GameObject.Find("Background").GetComponent<BoxCollider2D>();
            }
        }
		xMin = spawnArea.bounds.min.x;
		xMax = spawnArea.bounds.max.x;
		yMin = spawnArea.bounds.min.y;
		yMax = spawnArea.bounds.max.y;
		camOrthsize = cam.orthographicSize;
		camRatio = (xMax + camOrthsize) / 2.0f;
		BoxCollider2D enemyBounds = enemy.GetComponent<BoxCollider2D>();
		enemySizeX = enemyBounds.bounds.max.x - enemyBounds.bounds.center.x;
		enemySizeY = enemyBounds.bounds.max.y - enemyBounds.bounds.center.y;

	}

	// Update is called once per frame
	void Update()
	{
		if (World.isRunning)
		{

			if (spawnRate > maxSpawnrate)
            {
				spawnRate = initialSpawnRate - Mathf.Floor(World.timer / spawnrateScaleChunks) * spawnrateTimeScaling;
				//Debug.Log("step: " + (Mathf.Floor(World.timer / spawnrateScaleChunks)));
            } else
            {
				spawnRate = maxSpawnrate;
            }


			if (World.timer >= lastSpawn + spawnRate)
			{
				//Debug.Log("Spawning...");
				SpawnEnemyOutOfCamera(spawnOnGround);
				lastSpawn = World.timer;
			}
		} else
        {
			//Debug.Log("Timer: " + Time.deltaTime);
        }
	}



	// Randomly spawns enemies within bounds, but outside of the player's view. Higher
	// probability of enemies spawning just outside of the player's view.
	// TASKS TO DO:
	// Make sure that we're accounting for the size of the enemy (not just the center outside of the cam range, but the full enemy.
	void SpawnEnemyOutOfCamera(bool isGrounded)
    {
		Vector2 spawnPoint = GetOutOfCamPoint(isGrounded);
        if (spawnPoint.Equals(Vector2.positiveInfinity))
        {
            Debug.Log("failed to find spawn location -- skipping");
            return;
        }

        GameObject selectedEnemy = SelectEnemy();
		Instantiate(selectedEnemy, new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
		Debug.Log("Spawned at " + spawnPoint.ToString());
		if (spawnPoint.x > xMax || spawnPoint.x < xMin)
        {
			Debug.Log("WARNING! X: " + spawnPoint.x + " outside bounds: " + new Vector2(xMin, xMax));
        }
        if (spawnPoint.y > yMax || spawnPoint.y < yMin)
        {
            Debug.Log("WARNING! Y: " + spawnPoint.y + " outside bounds: " + new Vector2(yMin, yMax));
        }

    }

	public GameObject SelectEnemy()
    {
        int maxProb = normalProb;
        if (Time.time > timeSpawnStrong)
        {
            maxProb = (Time.time < timeSpawnStronger) ? strongProb : strongerProb;
        }
        int enemyProbability = Random.Range(0, maxProb);
        if (enemyProbability < 5)
        {
			return enemy;
        }
        else if (enemyProbability < 8)
        {
			return strongEnemy;
        }
        else
        {
			return strongerEnemy;
        }
    }

    public Vector2 GetOutOfCamPoint(bool grounded, int tryDepth = 0)
    {
        // Constraints:
        // 1. Enemies can't spawn in the player's view
        // // a. Must be outside of the range of (yCam + camOrthsize, yCam - camOrthsize) and (xMin + camRatio, xMax - camRatio)
        if (tryDepth > 5)
        {
            return Vector2.positiveInfinity;
        }

        float xOrY = Random.value;
        float xPosition = Random.value * 2.0f;
        float yPosition = Random.value * 2.0f;

        if (xOrY < 0.5f)
        {
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

		Vector2 spawnPoint = new Vector2(xPosition, yPosition);

		if (grounded)
        {
			LayerMask groundMask = LayerMask.GetMask("Ground");
			RaycastHit2D rayDown = Physics2D.Raycast(spawnPoint, Vector2.down, yPosition - yMin, groundMask);
			if (rayDown.collider != null)
            {
				spawnPoint.y = rayDown.collider.bounds.max.y + groundedSpawnOffset;
            } else
            {
				RaycastHit2D rayUp = Physics2D.Raycast(spawnPoint, Vector2.up, yMax - yPosition, groundMask);
				if (rayUp.collider != null)
                {
					spawnPoint.y = rayUp.collider.bounds.max.y + groundedSpawnOffset;
                } else
                {
                    Debug.Log("Couldn't find valid spawning area -- trying again!");
                    return GetOutOfCamPoint(grounded, tryDepth + 1);
                }
            }

        }

		return spawnPoint;
    }
}

