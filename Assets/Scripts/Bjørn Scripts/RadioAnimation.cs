using UnityEngine;
using UnityEngine.InputSystem;

public class RadioAnimation : MonoBehaviour
{
    [HideInInspector] public Animator radio;

    private void Start()
    {
        radio = GetComponent<Animator>();
    }

    public void SpeakerJumpLeft()
    {
        radio.Play("JumpLeftShort");
    }
    public void SpeakerJumpFarRight()
    {
        radio.Play("JumpFarRight");
    }
    public void SpeakerJumpFarLeft()
    {
        radio.Play("JumpFarLeft");
    }

    public void Idle()
    {
        radio.Play("Idle");
    }

    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
        {
            Music.PlayOneShot("SFX/ough_6", transform.position);
        }
    }
}
