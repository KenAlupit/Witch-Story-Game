using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject controlsMenuUI;
    public GameObject blackScreen;
    public GameObject deathScreen;
    public Transform cutsceneDetection;
    public EnemyKnightBehaviour knightEnemy;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene() != SceneManager.GetSceneByName("MainMenuScene"))
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !deathScreen.activeInHierarchy)
            {
                if (GameIsPaused)
                {
                    Resume();
                } else
                {
                    Pause();
                }
            }
        }
        if(cutsceneDetection != null)
        {
            RaycastHit2D rayInfo = Physics2D.Raycast(cutsceneDetection.position, Vector2.up);
            if (rayInfo.collider.tag.Equals("Player"))
            {
                if (knightEnemy.isDead)
                {
                    blackScreen.SetActive(true);
                    blackScreen.GetComponent<Animator>().SetTrigger("Fade");
                }
            }
        }

    }

    public void LoadGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void LoadEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public void ExitGame()
    {
        Debug.Log("Exiting");
        Application.Quit();
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        controlsMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}
