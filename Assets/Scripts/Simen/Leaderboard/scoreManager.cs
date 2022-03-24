using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class scoreManager : MonoBehaviour
{
    public TMP_Text countText;
    public transformVariable score;
    public int totalScore;

    #region PointSystem
    
    [Header("Point system")]
    [Header("Gain Points")] [Space(5)]
    [Range(0, 100)] public int killPoints;

    [Header("Loose Points")] [Space(5)] 
    [Range(0, 100)] public int loosePoints;

    [Header("Point per % off grass cut")] [Space(5)] 
    [Range(2, 10)] public int grassPoints;

    #endregion
    
    private void Start()
    {
        score.score = 0;
        score.score2 = 0;
    }

    private void Awake()
    {
        score.gainPointsFromKills = killPoints;
        score.loosePointsFromFriendlyKills = loosePoints;
    }

    private void Update()
    {
        SetCountText();
        print(score.score + score.score2);
        totalScore = (int) (score.score + score.score2);
    }
    
    void SetCountText()
    {
        if (countText != null)
        {
            countText.text = "Score: " + totalScore;
        }
    }
}