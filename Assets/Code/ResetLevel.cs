using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    private GameObject death;
    private DeathScreen deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        death = GameObject.Find("DeathScreen");
        deathScreen = death.GetComponent<DeathScreen>();
    }

    // Update is called once per frame
    void Update()
    {
        if (deathScreen.isGameOver())
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Jump"))
            {
                ReloadScene();
            }
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
