using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{

    // TODO: move this timer code to World component
    public static float timer;
    private float lastTimer;
    private TutorialTransition tutorialTransition;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;

        tutorialTransition = GameObject.Find("Tutorial Transition").GetComponent<TutorialTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialTransition.FinishedTutorialHuh())
        {
            timer += Time.deltaTime;
            var newTimer = Mathf.Floor(timer);
            if (newTimer > lastTimer)
            {
                Debug.Log("WORLD TIME: " + newTimer);
            }
            lastTimer = newTimer;

        }
    }
}
