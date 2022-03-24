using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class LawnmowerAudio : MonoBehaviour
{
    [SerializeField] private SphereMovement _sphere;

    private List<EventInstance?> audioSetup = new ();
    private EventInstance sfxIdle;
    private EventInstance sfxDrive;
    private EventInstance sfxReverse;

    private bool startedDriving;
    private bool stoppedDriving;
    private float timerStartedDriving;
    private float timerStoppedDriving;

    #region - PlayLogic -
    
    private void OnDestroy()
    {
        Music.StopLoop(sfxIdle);
        Music.StopLoop(sfxDrive);
        Music.StopLoop(sfxReverse);
    }

    private void OnApplicationQuit()
    {
        Music.StopLoop(sfxIdle);
        Music.StopLoop(sfxDrive);
        Music.StopLoop(sfxReverse);
    }

    private void OnDisable()
    {
        Music.Pause(sfxIdle);
        Music.Pause(sfxDrive);
        Music.Pause(sfxReverse);
        SceneController.swappedScene -= StopAudio;
    }

    private void OnEnable()
    {
        Music.Play(sfxIdle);
        SceneController.swappedScene += StopAudio;
    }

    private void StopAudio()
    {
        Music.StopLoop(sfxIdle);
        Music.StopLoop(sfxDrive);
        Music.StopLoop(sfxReverse);
    }
    
    #endregion
    
    // Start is called before the first frame update
    void Start()
    {
        audioSetup.Add(Music.PlayLoop("SFX/lawnmower_idle", transform));
        audioSetup.Add(Music.PlayLoop("SFX/lawnmower_driving_1", transform));
        audioSetup.Add(Music.PlayLoop("SFX/reverse", transform));

        if (audioSetup[0] != null) sfxIdle = (EventInstance) audioSetup[0];
        if (audioSetup[1] != null) sfxDrive = (EventInstance) audioSetup[1];
        if (audioSetup[2] != null) sfxReverse = (EventInstance) audioSetup[2];
    }
    
    void FixedUpdate()
    {
        Music.UpdateAudioPosition(sfxIdle, transform);
        Music.UpdateAudioPosition(sfxDrive, transform);
        Music.UpdateAudioPosition(sfxReverse, transform);

        //Driving forwards
        if (_sphere.speedInput >= 0.05f)
        {
            stoppedDriving = false;
            timerStoppedDriving = 0.3f;
            
            //Plays start driving sound once
            if (!startedDriving)
            {
                Music.PlayOneShot("SFX/lawnmower_start_1", transform.position);
                startedDriving = true;
            }

            //Waits with playing drive loop for a short while to transition with the start drive sound
            if (timerStartedDriving > 0f) timerStartedDriving -= Time.fixedDeltaTime;
            else Music.Play(sfxDrive);
            
            Music.Pause(sfxIdle);
        }
        //Plays stop driving sound once
        else if (timerStoppedDriving > 0f)
        {
            if (!stoppedDriving)
            {
                Music.PlayOneShot("SFX/lawnmower_stop_1", transform.position);
                stoppedDriving = true;
            }
            timerStoppedDriving -= Time.fixedDeltaTime;
            timerStartedDriving = 0.35f;
        }
        //Plays idle sound if not driving forwards
        else
        {
            Music.Pause(sfxDrive);
            timerStartedDriving = 0.35f;
        }
        
        //Plays reverse sound if reversing
        if (_sphere.speedInput <= -0.05f)
        {
            Music.Play(sfxReverse);
            Music.Play(sfxIdle);
            startedDriving = false;
        }
        else
        {
            Music.Pause(sfxReverse);
        }

        if (Mathf.Abs(_sphere.speedInput) < 0.05f)
        {
            Music.Play(sfxIdle);
            startedDriving = false;
        }
    }
}
