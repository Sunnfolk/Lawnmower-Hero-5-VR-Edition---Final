using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScoreData
{
    //This script can probably be marged with ScoreManager, but I had some weird issue when doing that so that's why it's seperate.
    //This script is serializable and not placed on a gameobject.

    public List<Score> scores;

    public ScoreData()
    {
        scores = new List<Score>();
    }
}
