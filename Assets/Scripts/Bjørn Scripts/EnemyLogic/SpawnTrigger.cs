using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] private enemySpawner _spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LawnMower")) return;
        _spawner.triggerEnter = true;
    }
}