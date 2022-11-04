using System;
using PlayerPreferences;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    [SerializeField] private DataController _Data;
    private float timer;
    private string action;
    private string targetScene;
    private bool doLoadNow;
    public static event Action swappedScene;
    
    public void LoadScene(string sceneName)
    {
        swappedScene?.Invoke();
        _Data.SetPlayerData();
        targetScene = sceneName;
        timer = 0.05f;
        doLoadNow = true;
        action = "load";
    }

    private void Awake()
    {
        if (pauseEffect.GameIsPaused)
        {
            pauseEffect.GameIsPaused = false;
        }
    }

    public void ResetScene()
    {
        swappedScene?.Invoke();
        _Data.SetPlayerData();
        timer = 0.05f;
        doLoadNow = true;
        action = "reset";
    }

    public void QuitGame()
    {
        swappedScene?.Invoke();
        _Data.SetPlayerData();
        timer = 0.05f;
        doLoadNow = true;
        action = "quit";
    }

    private void FixedUpdate()
    {
        if (timer > 0) timer -= Time.fixedDeltaTime;
        else if (doLoadNow)
        {
            doLoadNow = false;
            if (action == "load") SceneManager.LoadScene(targetScene);
            else if (action == "quit") Application.Quit();
            else SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
    
    
}