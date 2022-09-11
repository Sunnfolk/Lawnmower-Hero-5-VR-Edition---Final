using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PlayFab.ExperimentationModels;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    //Place script on an empty gameobject in your scene.
    
    private ScoreData sd;

    private void Awake()
    {
        //Uncomment this to delete all scores
        //PlayerPrefs.DeleteKey("scores");
        
        var json = PlayerPrefs.GetString("scores", "{}");
        //sd = new ScoreData();
        sd = JsonUtility.FromJson<ScoreData>(json);
        
    }

    public IEnumerable<Score> GetHighScores()
    {
        return sd.scores.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }

    private void OnDestroy()
    {
        SaveScore();
    }

    public void SaveScore()
    {
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("scores", json);
    }
    //This is the code we use to add score.
    //scoreManager.AddScore(new Score("Anders2", 200));
}
