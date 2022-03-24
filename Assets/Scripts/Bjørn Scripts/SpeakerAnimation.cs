using UnityEngine;
using UnityEngine.VFX;

public class SpeakerAnimation : MonoBehaviour
{
    private Animator speaker;
    [SerializeField] public VisualEffect _PSNotes;

    private void Start()
    {
        speaker = GetComponent<Animator>();
    }

    public void SpeakerBounce()
    {
        speaker.Play("Bounce");
    }

    public void Idle()
    {
        speaker.Play("Idle");
    }

    public void PlayPSNotes()
    {
        _PSNotes.Play();
    }

    public void StopPSNotes()
    {
        _PSNotes.Stop();
    }
}
