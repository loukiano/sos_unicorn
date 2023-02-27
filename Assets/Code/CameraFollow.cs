using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform followTransform;
    public BoxCollider2D mapBounds;

    public float maxYOff = 5;
    public float maxXOff = 5;

    public float moveDamp = 0.2f;

    private float xMin, xMax, yMin, yMax;
    private float camY, camX;
    private float camOrthsize;
    private float cameraRatio;
    private Camera mainCam;
    private TutorialTransition tutorialTransition;

    // Start is called before the first frame update
    void Start()
    {
        xMin = mapBounds.bounds.min.x;
        xMax = mapBounds.bounds.max.x;
        yMin = mapBounds.bounds.min.y;
        yMax = mapBounds.bounds.max.y;
        mainCam = GetComponent<Camera>();
        camOrthsize = mainCam.orthographicSize;
        cameraRatio = mainCam.aspect * camOrthsize;
        GameObject tutObj = GameObject.Find("Tutorial Transition");
        if (tutObj != null)
        {
            TutorialTransition tutTrans = tutObj.GetComponent<TutorialTransition>();
            if (!tutTrans.Equals(null))
            {
                Debug.Log(tutTrans.ToString());
                tutTrans.StartGame();
            }

        }

        followTransform = GameObject.Find("Player").transform;
    }

    void FixedUpdate()
    {
        if (tutorialTransition == null || tutorialTransition.FinishedTutorialHuh())
        {
            float xOff = Controller.GetAxisFilter("RightHorizontal") * maxXOff;
            float yOff = Controller.GetAxisFilter("RightVertical") * maxYOff;

            float newX = Mathf.Lerp(transform.position.x, followTransform.position.x + xOff, moveDamp);
            float newY = Mathf.Lerp(transform.position.y, followTransform.position.y + yOff, moveDamp);

            camX = Mathf.Clamp(newX, xMin + cameraRatio, xMax - cameraRatio);
            camY = Mathf.Clamp(newY, yMin + camOrthsize, yMax - camOrthsize);
            this.transform.position = new Vector3(camX, camY, this.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
