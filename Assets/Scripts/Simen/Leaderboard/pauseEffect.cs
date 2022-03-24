using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class pauseEffect : MonoBehaviour
{
    public static bool GameIsPaused;

    public GameObject PauseMenuUI;
    private Timer _timer;

    private void Start()
    {
        PauseMenuUI.SetActive(false);
        _timer = GetComponent<Timer>();
    }

    public void Resume()
    {
        if (!_timer.timerIsRunning)
        {
            return;
        }
        PauseMenuUI.SetActive(false);
        Music.SetParameter("Paused", 0);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    public void Pause()
    {
        PauseMenuUI.SetActive(true);
        Music.SetParameter("Paused", 1);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void ScorePause()
    {
        Time.timeScale = 0f;
        GameIsPaused = true;
    }
}