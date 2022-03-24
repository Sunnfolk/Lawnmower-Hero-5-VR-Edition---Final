using UnityEngine;
using UnityEngine.VFX;

public class EnemyHealth : MonoBehaviour, IPooledObject
{
    public float health, maxHealth = 10;
    public VisualEffect _Effect;
    public transformVariable _score;

    [HideInInspector] public int pool = -1;
    private ObjectPooler pooler;

    private void Start()
    {
        print("Start");
        
        pooler = ObjectPooler.Instance;

        _Effect.Stop();
    }

    //Resets health when reused
    public void OnObjectSpawn()
    {
        print("Spawned");
        health = maxHealth;
        
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (health <= 0)
        {
            _Effect.Play();
            Destroy(_Effect, 3f);
            _Effect.transform.parent = null;
          
            if (gameObject.CompareTag("EvilGnome"))
            {
                _score.score += _score.gainPointsFromKills;

                Music.PlayOneShot(Random.Range(0f, 2f) >= 1f ? "SFX/ceramic_break_1" : "SFX/ceramic_break_2", transform.position);
            }
            else if (gameObject.CompareTag("Wasp"))
            {
                _score.score += _score.gainPointsFromKills;
            }
            else if (gameObject.CompareTag("GoodGnome"))
            {
                _score.score -= _score.loosePointsFromFriendlyKills;

                Music.PlayOneShot(Random.Range(0f, 2f) >= 1f ? "SFX/ceramic_break_1" : "SFX/ceramic_break_2", transform.position);
            }
            else if (gameObject.CompareTag("Bee"))
            {
                _score.score -= _score.loosePointsFromFriendlyKills;
            }
            
            //TODO: Check if enemy deactivates when killed
            print("Died to Ded");

            if (pool >= 0)
            {
                //Removes itself from count in its pool
                pooler.pools[pool].activeObjects--;
            }
            
            gameObject.SetActive(false);

            //StartCoroutine(destroyEnemy());
        }
    }

   /* IEnumerator destroyEnemy()
    {
        yield return new WaitForSeconds((float) 0.25);
        Destroy(gameObject);
    }*/

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Pellet"))
        {
            health = 0;
            
            print("is realy supposed to be dead rn");
        }
    }
}