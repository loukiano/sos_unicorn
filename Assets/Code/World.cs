using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    public static float timer;
    private float lastTimer;
    public static bool isRunning;

    public bool hasTutorial;
    private TutorialTransition tutorialTransition;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        if (hasTutorial)
        {
            tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
        }
    }

    public static void StartGame()
    {
        isRunning = true;
        TutorialTransition tutTrans =  GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
        if (tutTrans != null)
        {
            tutTrans.StartGame();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (!isRunning && (!hasTutorial ||
                            (hasTutorial && tutorialTransition.FinishedTutorialHuh())))
        {
            isRunning = true;
        }
        if (isRunning)
        {
            timer += Time.deltaTime;
            var newTimer = Mathf.Floor(timer);
            if (newTimer > lastTimer)
            {
                //Debug.Log("WORLD TIME: " + newTimer);
            }
            lastTimer = newTimer;
        }
            
    }
}
