using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class SubmitScore : MonoBehaviour
{


    [SerializeField] private GameObject inputField;
    [SerializeField] private int score;
    [SerializeField] private CalculateCutGrass calculateCutGrass;
    [SerializeField] private TMP_Text scoreText;
    private ScoreManager _scoreManager;
    private string _name;

    private bool canFetchScore;

    //HEY LISTEN UP. MAYBE GET SCORE FROM THE CALCULATECUTGRASS ON AWAKE OR SOMETHING. 
    
    private void Start()
    {
        _scoreManager = this.GetComponent<ScoreManager>();
    }

    private void OnEnable()
    {
        score = Mathf.RoundToInt(calculateCutGrass.grassScore);
        scoreText.text = "Score: " + score;
        canFetchScore = true;
    }

    private void LateUpdate()
    {
        if (canFetchScore)
        {
            scoreText.text = "Score: " + score;
            score = Mathf.RoundToInt(calculateCutGrass.grassScore);
            if (score != 0)
            {
                canFetchScore = false;
                scoreText.text = "Score: " + score;
            }
        }
        
    }

    public void ScoreSubmitFunction()
    {
        print("ScoreSubmitFunction");
        _name = inputField.GetComponent<TMP_InputField>().text;
        if (name != null && score != null)
        {
            print("Scoremanager addscore incoming right now");
            print("Name: " + _name + " Score: " + score);
            _scoreManager.AddScore(new Score(_name, score));
            _scoreManager.SaveScore();
        }
        
        
    }

    private void Update()
    {
        if (Keyboard.current.pKey.wasPressedThisFrame)
        {
            print("SUBMIT SCORE NOW");
            ScoreSubmitFunction();
        }
    }
}
