using UnityEngine;

public class ExitSpawnTrigger : MonoBehaviour
{
    [SerializeField] private enemySpawner _spawner;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LawnMower")) return;
        _spawner.triggerExit = true;
    }
}