using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class pauseEffect : MonoBehaviour
{
    public static bool GameIsPaused;
    [SerializeField] private bool isPausedInspector;

    public GameObject PauseMenuUI;
    public GameObject ScoreUI;
    public Timer _timer;

    private void Start()
    {
        PauseMenuUI.SetActive(false);
        
    }

    private void Awake()
    {
        _timer = GetComponent<Timer>();
    }

    private void OnEnable()
    {
        Resume();
        Time.timeScale = 1f;
    }

    private void Update()
    {
        isPausedInspector = GameIsPaused;
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
        ScoreUI.SetActive(true);
    }
}