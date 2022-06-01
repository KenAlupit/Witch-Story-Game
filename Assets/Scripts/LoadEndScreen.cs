using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadEndScreen : MonoBehaviour
{
    public GameObject endScreen;
    private AudioSource walkSound;
    public float time = 0f;
    private bool startTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        walkSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (startTimer)
        {
            time += Time.deltaTime;
            if (time >= 5)
            {
                endScreen.SetActive(true);
                time = 0f;
            }
        }
    }

    public void Transition()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("EndScreen"))
        {
            startTimer = true;
            endScreen.GetComponent<Animator>().SetTrigger("Fade");
        } else if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("GameScene"))
        {
            SceneManager.LoadScene("EndScreen");
        }
    }
    public void Walk()
    {
        walkSound.Play();
    }
}
