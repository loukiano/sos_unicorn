using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemy;
	public GameObject strongEnemy;
	public GameObject strongerEnemy;

	public BoxCollider2D spawnArea;
    //public LevelDesign levelDesign;

    public bool shouldSpawn;

    public Camera cam;
    private Bounds camBounds;
	public float spawnRate; // interval betweeen enemy spawns
	public float initialSpawnRate;
    public float spawnrateTimeScaling;
	public float spawnrateScaleChunks;
	public float maxSpawnrate;

    public int maxNumChildren;

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
        camBounds = new Bounds(cam.transform.position, new Vector3(camOrthsize * cam.aspect, camOrthsize, 1));
		if (World.isRunning && shouldSpawn)
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
                string spawnMsg = transform.childCount + "/" + maxNumChildren + " children exist... ";
                if (transform.childCount < maxNumChildren)
                {
                    spawnMsg += "did spawn!";
				    SpawnEnemyOutOfCamera(spawnOnGround);
                } else
                {
                    spawnMsg += "didn't spawn!";
                }
                //Debug.Log(spawnMsg);
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


		GameObject newEnemy = Instantiate(SelectEnemy(), new Vector3(spawnPoint.x, spawnPoint.y, 0), Quaternion.identity);
        newEnemy.transform.parent = transform;

		
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
                //xPosition = Random.value * (cam.transform.position.x - camRatio - xMin) + xMin - enemySizeX;
                xPosition = Random.Range(xMin, camBounds.min.x - enemySizeX);
                if (xPosition < xMin)
                {
                    xPosition = xMin;
                }
            }
            // Spawn right of player
            else
            {
                //xPosition = Random.value * (xMax - (cam.transform.position.x + camRatio)) + cam.transform.position.x + camRatio + enemySizeX;
                xPosition = Random.Range(camBounds.max.x + enemySizeX, xMax);
                if (xPosition > xMax)
                {
                    xPosition = xMax;
                }
            }

            yPosition = Random.value * (yMax - yMin) + yMin;
        }
        else
        {
            // separate on y axis
            // Spawn above player
            if (yPosition <= 1.0f)
            {
                //yPosition = Random.value * (cam.transform.position.y - camOrthsize - yMin) + yMin - enemySizeY;
                yPosition = Random.Range(camBounds.max.y + enemySizeY, yMax);
                if (yPosition > yMax)
                {
                    yPosition = yMax;
                }
            }
            // Spawn below player
            else
            {
                //yPosition = Random.value * (yMax - (cam.transform.position.y + camOrthsize)) + cam.transform.position.y + camOrthsize - enemySizeY;
                yPosition = Random.Range(yMin, camBounds.min.y - enemySizeY);
                if (yPosition < yMin)
                {
                    yPosition = yMin;
                }
            }

            xPosition = Random.value * (xMax - xMin) + xMin;
        }

		Vector2 spawnPoint = new Vector2(xPosition, yPosition);

		if (grounded)
        {
			LayerMask groundMask = LayerMask.GetMask("Ground");
			RaycastHit2D rayOne = Physics2D.Raycast(spawnPoint, Vector2.down, yPosition - yMin, groundMask);
            RaycastHit2D rayTwo = Physics2D.Raycast(spawnPoint, Vector2.up, yMax - yPosition, groundMask);

            if (Random.value > 0.5)
                // half the time, check up first
            {
                RaycastHit2D temp = rayOne;
                rayOne = rayTwo;
                rayTwo = temp;
            }

            if (rayOne.collider != null)
            {
				spawnPoint.y = rayOne.collider.bounds.max.y + groundedSpawnOffset;
                
            } else
            {
				
				if (rayTwo.collider != null)
                {
					spawnPoint.y = rayTwo.collider.bounds.max.y + groundedSpawnOffset;
                } else
                {
                    Debug.Log("Couldn't find valid spawning area -- trying again!");
                    return GetOutOfCamPoint(grounded, tryDepth + 1);
                }
            }

            if ((spawnPoint.x > cam.transform.position.x - camOrthsize && spawnPoint.x < cam.transform.position.x + camOrthsize) ||
                (spawnPoint.y > cam.transform.position.y - camOrthsize && spawnPoint.y < cam.transform.position.y + camOrthsize))
                // new spawn is inside the camera :(
            {
                Debug.Log(spawnPoint.ToString() + "Would have spawned in camera -- trying again!");
                return GetOutOfCamPoint(grounded, tryDepth + 1);
            }

        }

        if (spawnPoint.x > xMax || spawnPoint.x < xMin)
        {
            Debug.Log("WARNING! X: " + spawnPoint.x + " outside bounds: " + new Vector2(xMin, xMax) +
                    "\n grounded = " + grounded + ", clamping to be in bounds");
            spawnPoint.x = spawnPoint.x > xMax ? xMax : xMin;
        }
        if (spawnPoint.y > yMax || spawnPoint.y < yMin)
        {
            Debug.Log("WARNING! Y: " + spawnPoint.y + " outside bounds: " + new Vector2(yMin, yMax) +
                    "\n grounded = " + grounded + ", clamping to be in bounds");
            spawnPoint.y = spawnPoint.y > yMax ? yMax :yMin;
        }




        return spawnPoint;
    }

    public void StartSpawn()
    {
        shouldSpawn = true;
    }

    public void StopSpawn()
    {
        shouldSpawn = false;
    }
}

