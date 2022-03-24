using System.Collections.Generic;
using PlayerPreferences;
using UnityEngine;

public class RadioDial : MonoBehaviour
{
    public float rotateSpd = 120f;
    public float channelWidth = 0.8f; //How close to the channel value you have to be for no whitenoise to appear
    public float whiteNoiseFadeWidth = 1f; //Distance from no whitenoise to full whitenoise outside each channel
    public float[] channelHertzValues;

    private float _minHertz = 88f;
    private float _maxHertz = 108f;
    private float _dialMinAngle = -140f;
    private float _dialMaxAngle = 140f;

    private float _yRot;

    private List<Transform> _handsTransforms = new List<Transform>();
    private bool _handSticked;
    private float _rotationOffset;
    private float _hackyMultiplier = 100f;
    private float tempRot;

    [SerializeField] private PlayerInput input;
    [SerializeField] private Music music; 
    [SerializeField] private DataController data;

    private void Awake()
    {
        var angle = (data.hertz - _minHertz) * (_dialMaxAngle - _dialMinAngle) / (_maxHertz - _minHertz);
        var rot = transform.localEulerAngles;
        
        transform.localRotation = Quaternion.Euler(rot.x, -angle - _dialMinAngle, rot.z);
        _yRot = -angle - _dialMinAngle;
    }
    
    private void OnStickedHandsChanged(InteractAble.Hand[] stickedHands)
    {
        foreach (InteractAble.Hand hand in stickedHands)
        {
            if (hand.Transform != null)
            {
                print(hand.Transform);
                
                _handsTransforms.Add(hand.Transform);
                
                _handSticked = true;
                CalculateOffset();
            }
            else
            {
                print(hand.LastFrameStickedHandTransform);
                
                _handsTransforms.Remove(hand.LastFrameStickedHandTransform);
                _handSticked = false;
            }
        }
    }

    private void CalculateOffset()
    {
        //_rotationOffset = _handsTransforms[0].rotation.z;
    }

    // Update is called once per frame
    private void Update()
    {
        var rot = transform.localEulerAngles;
        var add = rotateSpd * Time.deltaTime;
        if (_handSticked)
        {
            Mathf.Clamp(_yRot, _dialMinAngle, _dialMaxAngle);
            tempRot = _handsTransforms[0].rotation.z;
            _yRot = tempRot * _hackyMultiplier;
        }
        /*if (input.rotateDir > 0f)
        {
            if (_yRot + add <= _dialMaxAngle) _yRot += add;
            else _yRot = _dialMaxAngle;
        }
        if (input.rotateDir < 0f)
        {
            if (_yRot - add >= _dialMinAngle) _yRot -= add;
            else _yRot = _dialMinAngle;
        }*/
        
        transform.localRotation = Quaternion.Euler(rot.x, _yRot, rot.z);

        //Calculate Hertz
        var angle = -_yRot - _dialMinAngle;
        data.hertz = _minHertz + (angle / (_dialMaxAngle - _dialMinAngle) * (_maxHertz - _minHertz));
        
        //Use Hertz to select channel & whitenoise
        if (data.hertz <= 97.5f) music.SetChannel(0); //Channel at 93
        else music.SetChannel(1); //Channel at 102

        //Calculates whether to play whitenoise or not
        var fullWhitenoise = true;
        for (var i = 0; i < channelHertzValues.Length; i++)
        {
            var dist = Mathf.Abs(data.hertz - channelHertzValues[i]);
            //Plays music at full strength without whitenoise
            if (dist <= channelWidth)
            {
                Music.SetParameter("Whitenoise", 0);
                fullWhitenoise = false;
            }
            //Plays some music and some whitenoise
            else if (dist <= channelWidth + whiteNoiseFadeWidth)
            {
                Music.SetParameter("Whitenoise", (dist - channelWidth)/whiteNoiseFadeWidth);
                fullWhitenoise = false;
            }
        }
        //Only plays whitenoise
        if (fullWhitenoise) Music.SetParameter("Whitenoise", 1);
    }
}
