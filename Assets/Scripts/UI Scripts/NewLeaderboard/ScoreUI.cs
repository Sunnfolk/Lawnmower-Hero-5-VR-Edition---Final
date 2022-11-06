using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    //This script is placed on the "Content" gameobject in the main menu. It handles instantiating of the rows and inserting the scores
    
    public RowUI rowUI;
    public ScoreManager scoreManager;

    private void Start()
    {
        if (!transform.parent.CompareTag("LevelTwoScore"))
        {
            var scores = scoreManager.GetHighScores().ToArray();
            print("scores " + scores.Length);
            for (int i = 0; i < scores.Length; i++)
            {
                var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
                row.rank.text = (i + 1).ToString();
                row.name.text = scores[i].name;
                row.score.text = scores[i].score.ToString();
            }
        }
        else
        {
            print("GOOOOOSDKALSKDJLAKSJDLAKJS");
            var scoresTwo = scoreManager.GetHighScoresTwo().ToArray();
            print("scorestwooooo " + scoresTwo.Length);
            for (int i = 0; i < scoresTwo.Length; i++)
            {
                print("Adding score");
                var row = Instantiate(rowUI, transform).GetComponent<RowUI>();
                row.rank.text = (i + 1).ToString();
                row.name.text = scoresTwo[i].name;
                row.score.text = scoresTwo[i].score.ToString();
            }
        }
    }
}
