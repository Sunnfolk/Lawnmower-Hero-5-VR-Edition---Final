using UnityEngine;
using Random = UnityEngine.Random;

public class CrashAudio : MonoBehaviour
{
    public void Crashed()
    {
        var val = Random.Range(0,3f);

        if (val < 1) Music.PlayOneShot("SFX/crash_1", transform.position);
        else if (val < 2) Music.PlayOneShot("SFX/crash_3", transform.position);
        else Music.PlayOneShot("SFX/crash_5", transform.position);
    }
}
