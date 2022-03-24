using UnityEngine;

public class runOverNest : MonoBehaviour
{
    [SerializeField] private Collider deathTrigger;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("LawnMower"))
        {
            Music.PlayOneShot("SFX/crunch_1", transform.position);
            Destroy(gameObject);
        }
    }
}