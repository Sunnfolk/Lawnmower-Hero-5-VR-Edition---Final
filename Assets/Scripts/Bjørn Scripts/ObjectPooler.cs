using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    
    //Class to create new pools
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
        [HideInInspector] public int activeObjects;
    }
    
    #region - Singleton -
    
    public static ObjectPooler Instance;

    private void Awake()
    {
        Instance = this;
        
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        //Adds pools to dictionary
        foreach (var pool in pools)
        {
            var objectPool = new Queue<GameObject>();

            //Instantiates the objects needed for each pool, and adds it to the end of its queue
            for (int i = 0; i < pool.size; i++)
            {
                var obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                objectPool.Enqueue(obj);
            }
            
            poolDictionary.Add(pool.tag, objectPool);
        }
    }
    
    #endregion - Singleton -
    
    public List<Pool> pools; //List of created pools
    public Dictionary<string, Queue<GameObject>> poolDictionary; //Dictionary of all the pools

    //Spawns an object from pool
    public GameObject SpawnFromPool(string tag, Vector3 pos, Quaternion rot)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        var objToSpawn = poolDictionary[tag].Dequeue();
        print(objToSpawn);
        objToSpawn.transform.position = pos;
        objToSpawn.transform.rotation = rot;
        
        objToSpawn.SetActive(true);

        var pooledObj = objToSpawn.GetComponent<IPooledObject>();
        
        if (pooledObj != null)
        {
            pooledObj.OnObjectSpawn();
        }

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

    private void Update()
    {
        print(pools[0].activeObjects + " : " + pools[1].activeObjects);
    }
}