using FMOD.Studio;
using UnityEngine;

public class WaspAudio : MonoBehaviour
{
    private EventInstance sfxBuzz;

    #region - PlayLogic -
    
    private void OnDestroy()
    {
        Music.StopLoop(sfxBuzz);
    }

    private void OnApplicationQuit()
    {
        Music.StopLoop(sfxBuzz);
    }

    private void OnDisable()
    {
        Music.Pause(sfxBuzz);
        SceneController.swappedScene -= StopAudio;
    }

    private void OnEnable()
    {
        Music.Play(sfxBuzz);
        SceneController.swappedScene += StopAudio;
    }
    
    private void StopAudio()
    {
        Music.StopLoop(sfxBuzz);
    }
    
    #endregion
    
    void Start()
    {
        var inst = Music.PlayLoop("SFX/Buzz", transform);

        if (inst != null) sfxBuzz = (EventInstance) inst;
    }
    
    private void FixedUpdate()
    {
        Music.UpdateAudioPosition(sfxBuzz, transform);
    }
}
