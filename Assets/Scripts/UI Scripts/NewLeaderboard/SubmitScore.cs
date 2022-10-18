using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SubmitScore : MonoBehaviour
{


    [SerializeField] private GameObject inputField;
    [SerializeField] private int score;
    private ScoreManager _scoreManager;
    private string _name;

    //HEY LISTEN UP. MAYBE GET SCORE FROM THE CALCULATECUTGRASS ON AWAKE OR SOMETHING. 
    
    private void Start()
    {
        _scoreManager = this.GetComponent<ScoreManager>();
    }


    public void ScoreSubmitFunction()
    {
        _name = inputField.GetComponent<TMP_InputField>().text;
        score = 1;
        if (name != null && score != null)
        {
            _scoreManager.AddScore(new Score(_name, score));
        }
        
        
    }
}
