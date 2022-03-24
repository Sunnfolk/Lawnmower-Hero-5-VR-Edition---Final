using PlayerPreferences;
using UnityEngine;

public class dataSetup : MonoBehaviour
{
    [SerializeField] private DataController _Data;
    
    private void Awake()
    {
        _Data.GetPlayerData();
    }

    private void OnDestroy()
    {
        _Data.SetPlayerData();
    }
}