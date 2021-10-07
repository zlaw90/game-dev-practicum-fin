using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PauseControl : MonoBehaviour
{
    public static event Action GameWasPaused;
    public static event Action GameWasResumed;
    private bool _paused = false;
    public GameObject pauseMenu;
    public GameObject inGameMenu;
    public GameObject buyMenu;
    public AudioSource gameAudio;

    public void OnPause()
    {
        if (!_paused)
            PauseGame();
        else
            ResumeGame();
    }
    public void PauseGame()
    {
        _paused = true;
        Time.timeScale = 0f;
        GameWasPaused?.Invoke();
        gameAudio.Pause();
        pauseMenu.SetActive(true);
        inGameMenu.SetActive(false);
        buyMenu.SetActive(false);
    }
    public void ResumeGame()
    {
        _paused = false;
        Time.timeScale = 1f;
        GameWasResumed?.Invoke();
        gameAudio.Play();
        pauseMenu.SetActive(false);
        inGameMenu.SetActive(true);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
