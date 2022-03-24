using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    private float oughTimer;
    [SerializeField] private float minOughCooldown = 2f;
    [SerializeField] private float maxOughCooldown = 4f;

    [SerializeField] private float superOughChance = 200f;

    private string[] sfxDamagedNames =
        {"SFX/Ough_1", "SFX/Ough_2", "SFX/Ough_3", "SFX/Ough_4", "SFX/Ough_5", "SFX/Ough_7", "SFX/Ough_8"};

    #region - PlayLogic -
    
    private void OnDestroy()
    {
        //Music.StopLoop();
    }

    private void OnApplicationQuit()
    {
        //Music.StopLoop();
    }

    private void OnDisable()
    {
        //Music.Pause();
    }

    private void OnEnable()
    {
        //Music.Play();
    }
    
    #endregion

    private void FixedUpdate()
    {
        if (SphereMovement.EnemiesInRange > 0)
        {
            if (oughTimer > 0) oughTimer -= Time.fixedDeltaTime;
            else
            {
                Music.PlayOneShot(
                    Random.Range(0, superOughChance) <= 1f ? 
                        "SFX/Ough_6" : sfxDamagedNames[Random.Range(0, sfxDamagedNames.Length)], transform.position);

                oughTimer = Random.Range(minOughCooldown, maxOughCooldown);
            }
        }
    }
}
