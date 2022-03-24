using UnityEngine;
using Random = UnityEngine.Random;

public class enemySpawner : MonoBehaviour
{
    public int poolUsed;
    
    [SerializeField] private int minSpawnCount = 1;
    [SerializeField] private int maxSpawnCount = 3;

    [HideInInspector] public bool triggerEnter;
    [HideInInspector] public bool triggerExit;

    private ObjectPooler pooler;
    private bool canSpawn = true;
    
    private void Start()
    {
        pooler = ObjectPooler.Instance;
    }

    private void FixedUpdate()
    {
        if (triggerEnter) EnteredTrigger();
        else if (triggerExit) ExitedTrigger();
    }

    private void EnteredTrigger()
    {
        triggerEnter = false;

        if (!canSpawn || pooler.pools[poolUsed].activeObjects >= pooler.pools[poolUsed].size)
        {
            return;
        }
            
        canSpawn = false;
        var count = Random.Range(minSpawnCount, maxSpawnCount);
        var tag = "";
        if (poolUsed == 0) tag = "gnome";
        else tag = "wasp";

        for (var i = 0; i < count; i++)
        {
            var obj = pooler.SpawnFromPool(tag,
                transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 2f, Random.Range(-0.5f, 0.5f)),
                Quaternion.identity);

            obj.GetComponent<EnemyHealth>().pool = poolUsed;
            
            pooler.pools[poolUsed].activeObjects++;
            
            if (pooler.pools[poolUsed].activeObjects >= pooler.pools[poolUsed].size) break; //Exits loop if all objects in pool are activated
        }
    }

    private void ExitedTrigger()
    {
        triggerExit = false;
        canSpawn = true;
    }

    //Is only run by wasp nests
    public void DeathTrigger()
    {
        Destroy(gameObject);
    }
}
