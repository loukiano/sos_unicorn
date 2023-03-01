using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class EnemySpawner : MonoBehaviour
{
	public GameObject enemy;
	public GameObject strongEnemy;
	public GameObject strongerEnemy;

	public BoxCollider2D spawnArea;

    public GameObject objOnClear;
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

    public bool isClearableArea;
    private bool isOccupied;
    private float clearTime;
    public float loadCooldown;
    public float loadDist;
    public float unloadDist;


	private int normalProb = 5;
	private int strongProb = 8;
	private int strongerProb = 10;

	// Use this for initialization
	void Start()
	{
        clearTime = 0;
        if (cam == null)
        {
            cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

		if (spawnArea == null)
        {
			spawnArea = GetComponent<BoxCollider2D>();
			if (spawnArea == null)
            {
                Debug.Log("Resorting to background bounds");
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

        if (unloadDist < loadDist)
            // ensure threshold for unloading is greater than loading
        {
            unloadDist = loadDist + 1;
        }

	}

	// Update is called once per frame
	void Update()
	{
        camBounds = new Bounds(cam.transform.position, new Vector3(camOrthsize * cam.aspect, camOrthsize, 1));
		if (World.isRunning)
		{
            if (isClearableArea)
            {
                if (!isOccupied)
                {
                    MaybePopulateArea();
                } else
                {
                    CheckPopulation();
                }
            }
            else if (shouldSpawn)
            {
                MaybeSpawnEnemy();
            }
		} else
        {
			//Debug.Log("Timer: " + Time.deltaTime);
        }
	}

    void MaybePopulateArea()
    {
        // if the area needs to be loaded and isn't already full
        if (!IsFull() && CamDistSqr() < Mathf.Pow(loadDist, 2))
        {
            //populate till full
            int numToSpawn = maxNumChildren - transform.childCount;
            for (int i = 0; i < numToSpawn; i++)
            {
                SpawnEnemyOutOfCamera(spawnOnGround);
            }
            isOccupied = true;
            Debug.Log("Player closer than " + loadDist + " to " + gameObject.name + "; populated with " + transform.childCount + " enemies");
        }
    }

    void CheckPopulation()
    {
        if (clearTime == 0)
            // if this area hasn't been cleared
        {
            if (CamDistSqr() > Mathf.Pow(unloadDist, 2))
                // if the player is too far away, unload the enemies
            {
                Debug.Log("Player farther than " + unloadDist + " away; depopulating remaining enemies in " + gameObject.name);
                DepopulateArea();
            }
            else if (transform.childCount <= 0)
                // no more enemies -- area cleared!
            {
                clearTime = Time.time;
                Debug.Log(gameObject.name + " area cleared at " + clearTime);
                OnClear();

            }
        }
        else if (clearTime + loadCooldown < Time.time && CamDistSqr() >= Mathf.Pow(loadDist, 2))
        {
            // reset to be able to populate again
            Debug.Log(gameObject.name + " area now populatable again");
            isOccupied = false;
            clearTime = 0;
        }
    }

    void DepopulateArea()
    { 
        for (var i = transform.childCount - 1; i >= 0; i--)
        {
            GameObject.Destroy(transform.GetChild(i).gameObject);
        }
        clearTime = 1;
        isOccupied = false;
    }

    float CamDistSqr()
    {
        if (spawnArea.bounds.Contains(new Vector2(cam.transform.position.x, cam.transform.position.y)))
            // if inside spawnarea, dist is 0
        {
            return 0;
        }
        float xDist = Mathf.Abs(transform.position.x - cam.transform.position.x) - spawnArea.bounds.extents.x;
        float yDist = Mathf.Abs(transform.position.y - cam.transform.position.y) - spawnArea.bounds.extents.y;
        return Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2);
    }

    void MaybeSpawnEnemy()
    {
        if (spawnRate > maxSpawnrate)
        {
            spawnRate = initialSpawnRate - Mathf.Floor(World.timer / spawnrateScaleChunks) * spawnrateTimeScaling;
            //Debug.Log("step: " + (Mathf.Floor(World.timer / spawnrateScaleChunks)));
        }
        else
        {
            spawnRate = maxSpawnrate;
        }


        if (World.timer >= lastSpawn + spawnRate)
        {
            //Debug.Log("Spawning...");
            string spawnMsg = transform.childCount + "/" + maxNumChildren + " children exist... ";
            if (!IsFull())
            {
                spawnMsg += "did spawn!";
                SpawnEnemyOutOfCamera(spawnOnGround);
            }
            else
            {
                spawnMsg += "didn't spawn!";
            }
            Debug.Log(spawnMsg);
            lastSpawn = World.timer;
        }
    }

    private bool IsFull()
    {
        return transform.childCount > maxNumChildren;
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
        var enemyAi = newEnemy.GetComponent<AIController>();
        if (enemyAi != null)
        {
            enemyAi.spawnArea = spawnArea;
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
                //xPosition = Random.value * (cam.transform.position.x - camRatio - xMin) + xMin - enemySizeX;
                xPosition = Random.Range(xMin, camBounds.min.x - enemySizeX);
            }
            // Spawn right of player
            else
            {
                //xPosition = Random.value * (xMax - (cam.transform.position.x + camRatio)) + cam.transform.position.x + camRatio + enemySizeX;
                xPosition = Random.Range(camBounds.max.x + enemySizeX, xMax);
            }

            if (xPosition > xMax || xPosition < xMin)
            {
                xPosition = Random.Range(xMin, xMax);
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
            }
            // Spawn below player
            else
            {
                //yPosition = Random.value * (yMax - (cam.transform.position.y + camOrthsize)) + cam.transform.position.y + camOrthsize - enemySizeY;
                yPosition = Random.Range(yMin, camBounds.min.y - enemySizeY);
                
            }

            if (yPosition > yMax || yPosition < yMin)
            {
                yPosition = Random.Range(yMin, yMax);
            }

            xPosition = Random.value * (xMax - xMin) + xMin;
        }

		Vector2 spawnPoint = new Vector2(xPosition, yPosition);
        //spawnPoint += transform.localToWorldMatrix
        //Debug.Log("spawnPoint: " + spawnPoint);

		if (grounded)
        {
            spawnPoint = MakeGrounded(spawnPoint);
            if (spawnPoint == Vector2.negativeInfinity)
            {
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

    public Vector2 MakeGrounded(Vector2 point)
    {
        LayerMask groundMask = LayerMask.GetMask("Ground");
        RaycastHit2D rayOne = Physics2D.Raycast(point, Vector2.down, point.y - yMin, groundMask);
        RaycastHit2D rayTwo = Physics2D.Raycast(point, Vector2.up, yMax - point.y, groundMask);

        if (Random.value > 0.5)
        // half the time, check up first
        {
            RaycastHit2D temp = rayOne;
            rayOne = rayTwo;
            rayTwo = temp;
        }

        if (rayOne.collider != null)
        {
            point.y = rayOne.collider.bounds.max.y + groundedSpawnOffset;

        }
        else
        {

            if (rayTwo.collider != null)
            {
                point.y = rayTwo.collider.bounds.max.y + groundedSpawnOffset;
            }
            else
            {
                Debug.Log("Couldn't find valid spawning area -- trying again!");
                return Vector2.negativeInfinity;
            }
        }


        if ((point.x > cam.transform.position.x - camOrthsize && point.x < cam.transform.position.x + camOrthsize) &&
            (point.y > cam.transform.position.y - camOrthsize && point.y < cam.transform.position.y + camOrthsize))
        // new spawn is inside the camera :(
        {
            Debug.Log(point.ToString() + "Would have spawned in camera -- trying again!");
            return Vector2.negativeInfinity; ;
        }

        return point;
    }

    public void OnClear()
    {
        StartCoroutine(ExplodeHella());
    }

    private IEnumerator ExplodeHella()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject obj = Instantiate(objOnClear, spawnArea.bounds.center, Quaternion.identity);
            obj.transform.localScale *= 5;
            yield return new WaitForSeconds(.5f);
        }
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

