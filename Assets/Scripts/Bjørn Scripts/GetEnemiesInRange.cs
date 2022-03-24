using System;
using System.Collections.Generic;
using UnityEngine;

public class GetEnemiesInRange : MonoBehaviour
{
    private List<Collider> colliders = new();

    private void Update()
    {
        var count = 0;
        print(colliders.Count);
        
        foreach (var i in colliders)
        {
            if (i.CompareTag("EvilGnome") || i.CompareTag("Wasp"))
            {
                count++;
            }
        }
        
        SphereMovement.EnemiesInRange = count;
    }

    private void OnTriggerEnter (Collider other)
    {
        if (!colliders.Contains(other))
        {
            if (other.CompareTag("Wasp") || other.CompareTag("EvilGnome")) colliders.Add(other);
        }
    }
 
    private void OnTriggerExit (Collider other)
    {
        colliders.Remove(other);
    }
}
