using UnityEngine;

[CreateAssetMenu]
public class transformVariable : ScriptableObject
{
    public Transform playerTransform;
    public int score;
    public float score2;
    public int enemiesInRange;

    [Range(1, 100)]public int gainPointsFromKills;
    [Range(1, 100)]public int loosePointsFromFriendlyKills;
}
