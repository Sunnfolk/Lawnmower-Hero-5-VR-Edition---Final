using UnityEngine;

public class NestDeathTrigger : MonoBehaviour
{
    [SerializeField] private enemySpawner _spawner;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("LawnMower"))
        {
            return;
        }
        _spawner.DeathTrigger();
    }
}