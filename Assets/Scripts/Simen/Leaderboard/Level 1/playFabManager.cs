using System;
using System.Collections;
using System.Collections.Generic;
using FMOD;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.Networking;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using TMPro;
using UnityEngine.SceneManagement;
using Valve.VR;
using Random = Unity.Mathematics.Random;

public class playFabManager : MonoBehaviour
{
     #region Components

    [Header("Windows")] 
    public GameObject nameWindow;
    public GameObject leaderboardWindow;

    [Header("Display name window")] 
    public TMP_InputField nameInput;
    
    [Header("Leaderboard")]
    public GameObject rowPreFab;
    public Transform rowParent;
    [Space(5)] 
    public GameObject rowPreFabHighScore;
    public Transform firstPlace;

    private string _loggedInPlayFabId;
    public Timer _timer;
    private scoreManager _scoreController;
    private pauseEffect _pMenu;
    [SerializeField] private SceneController _sceneController;

    #endregion
    private void Start()
    {
        if (nameWindow != null)
        {
            nameWindow.SetActive(false);
        }
        if (leaderboardWindow != null)
        {
            leaderboardWindow.SetActive(false);
        }
        _scoreController = GetComponent<scoreManager>();
        _pMenu = GetComponent<pauseEffect>();
        Login();
        StartCoroutine(GetLeaderboardOnStart());
    }

    private void Awake()
    {
        StartCoroutine(GetLeaderboardOnStart());
    }

    private void Update()
    {
        if (_timer != null)
        {
            if (_timer.timerIsRunning == false)
            {
                SendLeaderboard(_scoreController.score.score);
            }
        
            if (_timer.canSubmitScore == true)
            {
                SetYourName();
                _timer.canSubmitScore = false;
            }
        }
    }

    void Login()
    {
        var request = new LoginWithCustomIDRequest()
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams()
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        _loggedInPlayFabId = result.PlayFabId;
        Debug.Log("Successful login/account create!");
        string name = null;
        if (result.InfoResultPayload.PlayerProfile != null)
        {
            name = result.InfoResultPayload.PlayerProfile.DisplayName;
        }
        print(_loggedInPlayFabId);
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest()
        {
            Statistics = new List<StatisticUpdate>()
            {
                new StatisticUpdate()
                {
                    StatisticName = "Easy",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderBoardUpdate, OnError);
    }

    void OnLeaderBoardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "Easy",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderoardGet, OnError);
    }

    void OnLeaderoardGet(GetLeaderboardResult result)
    {
        if (rowParent == null) return;
        
        foreach (Transform item in rowParent)
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPreFab, rowParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            Debug.Log(string.Format("Place: {0} | ID: {1} | VALUE: {2}", item.Position, item.PlayFabId, item.StatValue));
        }
    }
    
    public void GetFirstPlace()
    {
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "Easy",
            StartPosition = 0,
            MaxResultsCount = 1
        };
        PlayFabClientAPI.GetLeaderboard(request, OnFirstPlaceGet, OnError);
    }
    
    void OnFirstPlaceGet(GetLeaderboardResult result)
    {
        if (firstPlace == null) return;
        
            foreach (Transform item in firstPlace)
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPreFabHighScore, firstPlace);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            Debug.Log(string.Format("Place: {0} | ID: {1} | VALUE: {2}", item.Position, item.PlayFabId, item.StatValue));
        }
    }

    public void GetLeaderboardAroundPlayer()
    {
        var request = new GetLeaderboardAroundPlayerRequest()
        {
            StatisticName = "Easy",
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayer, OnError);
    }
    
    void OnLeaderboardAroundPlayer(GetLeaderboardAroundPlayerResult result)
    {
        foreach (Transform item in rowParent)
        {
            Destroy(item.gameObject);
        }
        
        foreach (var item in result.Leaderboard)
        {
            GameObject newGo = Instantiate(rowPreFab, rowParent);
            TMP_Text[] texts = newGo.GetComponentsInChildren<TMP_Text>();
            
            texts[0].text = (item.Position + 1).ToString();
            texts[1].text = item.DisplayName;
            texts[2].text = item.StatValue.ToString();

            if (item.PlayFabId == _loggedInPlayFabId)
            {
                texts[0].color = Color.magenta;
                texts[1].color = Color.magenta;
                texts[2].color = Color.magenta;
            }

            Debug.Log(string.Format("Place: {0} | ID: {1} | VALUE: {2}", item.Position, item.PlayFabId, item.StatValue));
        }
    }

    public void SubmitNameButton()
    {
        var request = new UpdateUserTitleDisplayNameRequest()
        {
            DisplayName = nameInput.text,
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnError);
        PullUpLeaderboard();
        _pMenu.Resume();
        Time.timeScale = 1f;
        _sceneController.LoadScene("MainMenu");
        print("About to load the main menu");
        
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Updated display name!");
        leaderboardWindow.SetActive(true);
    }
    
    private IEnumerator GetLeaderboardOnStart()
    {
        yield return new WaitForSeconds(1);
        GetLeaderboard();
        GetFirstPlace();
        while (true)
        {
            yield return new WaitForSeconds(60);
            GetLeaderboard();
            GetFirstPlace();
        }
    }

    public void SetYourName()
    {
        nameWindow.SetActive(true);
        if (leaderboardWindow != null)
        {
            leaderboardWindow.SetActive(false);
        }
    }

    public void PullUpLeaderboard()
    {
        nameWindow.SetActive(false);
        
        if (leaderboardWindow != null)
        {
            leaderboardWindow.SetActive(true);
        }
    }
}