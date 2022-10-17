using System.Collections;
using FMOD.Studio;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;


[RequireComponent(typeof(InteractAble))]
public class SpratActive : MonoBehaviour
{
    public GameObject _obj;
    public VisualEffect SprayEffect;

    private EventInstance? sfxSetup;
    private EventInstance sfxSpray;

    private bool _isHeld;
    //private bool _trackpadButtonDown;
    
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

    private void OnHeldByHandChanged(InteractAble.Hand heldByHand)
    {
        _isHeld = heldByHand.GameObject != null;
    }

    private void Start()
    {
        SprayEffect.Stop();

        sfxSetup = Music.PlayLoop("SFX/bugspray", transform);
        if (sfxSetup != null) sfxSpray = (EventInstance) sfxSetup;
        Music.Pause(sfxSpray);
    }

    //Anders note: Change back to OnTriggerButtonChanged if something breaks
    private void OnTriggerButtonChanged(bool trackpadButtonState)
    {
        //_trackpadButtonDown = trackpadButtonState;

        if (!trackpadButtonState)
        {
            Music.Pause(sfxSpray);
            SprayEffect.Stop();
            _obj.SetActive(false);
        }
        else if (_isHeld)
        {
            Music.Play(sfxSpray);
            SprayEffect.Play();
            StartCoroutine(SprayDelay());
        }
    }


    //"only" used for testing"
    /*void Update()
    {
        if (Keyboard.current.aKey.isPressed)
        {
            Music.Play(sfxSpray);
            SprayEffect.Play();
            StartCoroutine(SprayDelay());
        }
        else 
        {
            Music.Pause(sfxSpray);
            SprayEffect.Stop();
            _obj.SetActive(false);
        }
        
    }*/

    private IEnumerator SprayDelay()
    {
        yield return new WaitForSeconds(1);
        _obj.SetActive(true);
    }
}
