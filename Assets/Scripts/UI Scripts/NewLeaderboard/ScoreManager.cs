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
    private ScoreData sd2;

    private void Awake()
    {
        //Uncomment this to delete all scores
        //PlayerPrefs.DeleteKey("scores");
        
        var json = PlayerPrefs.GetString("scores", "{}");
        var json2 = PlayerPrefs.GetString("scorestwo", "{}");
        print("JSON1 is " +json);
        print("JSON2 is " +json2);
        //sd = new ScoreData();
        sd = JsonUtility.FromJson<ScoreData>(json);
        sd2 = JsonUtility.FromJson<ScoreData>(json2);

    }

    public IEnumerable<Score> GetHighScores()
    {
        print(sd.scores.OrderByDescending(x => x.score));
        return sd.scores.OrderByDescending(x => x.score);
        print(sd.scores.OrderByDescending(x => x.score));
    }
    
    public IEnumerable<Score> GetHighScoresTwo()
    {
        print("getHighscorestwoisrunningnow");
        print(sd2.scoreTwo.OrderByDescending(x => x.score));
        return sd2.scoreTwo.OrderByDescending(x => x.score);
    }

    public void AddScore(Score score)
    {
        sd.scores.Add(score);
    }
    public void AddScoreTwo(Score scoretwo)
    {
        sd2.scoreTwo.Add(scoretwo);
    }

    private void OnDestroy()
    {
        //Maybe if check what level you are on before doing this
        SaveScore();
        SaveScoreLevelTwo();
    }

    public void SaveScore()
    {
        var json = JsonUtility.ToJson(sd);
        PlayerPrefs.SetString("scores", json);
    }

    public void SaveScoreLevelTwo()
    {
        var json2 = JsonUtility.ToJson(sd2);
        PlayerPrefs.SetString("scorestwo", json2);
    }
    //This is the code we use to add score.
    //scoreManager.AddScore(new Score("Anders2", 200));
    
    //scoreManager.AddScoreTwo(new Score("Anders2", 200));
}
