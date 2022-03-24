using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class GnomeAudio : MonoBehaviour
{
    [SerializeField] private float minTimeUntilIdleSFX;
    [SerializeField] private float maxTimeUntilIdleSFX;
    
    private List<EventInstance?> audioSetup = new ();
    private EventInstance sfxMove;
    private EnemyMovement move;

    private float timeUntilIdleSFX;
    private string[] idleSfxNames = {"SFX/Gnome_Growl_1","SFX/Gnome_Growl_2","SFX/Gnome_Growl_3","SFX/Gnome_Growl_4","SFX/Gnome_Growl_5","SFX/Gnome_Growl_6"};
    
    #region - PlayLogic -
    
    private void OnDestroy()
    {
        Music.StopLoop(sfxMove);
    }

    private void OnApplicationQuit()
    {
        Music.StopLoop(sfxMove);
    }

    private void OnDisable()
    {
        Music.Pause(sfxMove);
        SceneController.swappedScene -= StopAudio;
    }

    private void OnEnable()
    {
        Music.Play(sfxMove);
        SceneController.swappedScene += StopAudio;
    }

    private void StopAudio()
    {
        Music.StopLoop(sfxMove);
    }
    
    #endregion
    
    private void Start()
    {
        audioSetup.Add(Music.PlayLoop("SFX/walking_grass_1", transform));

        if (audioSetup[0] != null) sfxMove = (EventInstance) audioSetup[0];
        
        move = GetComponent<EnemyMovement>();
        timeUntilIdleSFX = Random.Range(minTimeUntilIdleSFX, maxTimeUntilIdleSFX);
    }
    
    private void FixedUpdate()
    {
        Music.UpdateAudioPosition(sfxMove, transform);
        
        if (move.inCombat)
        {
            Music.Play(sfxMove);

            if (timeUntilIdleSFX <= 0)
            {
                Music.PlayOneShot(idleSfxNames[Random.Range(0, idleSfxNames.Length)], transform.position);

                timeUntilIdleSFX = Random.Range(minTimeUntilIdleSFX, maxTimeUntilIdleSFX);
            }
            else timeUntilIdleSFX -= Time.deltaTime;
        }
        else
        {
            Music.Pause(sfxMove);
        }
    }
}
