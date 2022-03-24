using UnityEngine;

public class SphereCollision : MonoBehaviour
{
    [SerializeField] private CrashAudio crashAudio;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Obstacle"))
        {
            crashAudio.Crashed();
        }
    }
}
