using System;
using UnityEngine;

public class InteractAble : MonoBehaviour
{
    /*
    Everything with this script will get messages with information about interactions
    including:
        private void OnTriggerButtonChanged(bool state)
        private void OnTrackpadButtonChanged(bool state)
        private void OnMenuButtonChanged(bool state)
        private void OnGripButtonChanged(bool state)
        private void OnHeldByHandChanged(InteractAble.hand hand)
        private void OnStickedHandsChanged(InteractAble.hand[] hands)
    */
    
    public class Hand //class containing data about the hand
    {
        public GameObject GameObject { get; }
        public Transform Transform { get; }
        public GameObject StickedToGameObject { get; }
        public GameObject LastFrameStickedHandGameObject { get; }
        public Transform LastFrameStickedHandTransform { get; }

        public Hand(GameObject GameObject = null, GameObject StickedToGameObject = null, GameObject LastFrameStickedHandGameObject = null)
        {
            this.GameObject = GameObject;
            this.Transform = (GameObject == null) ? null : GameObject.transform;
            this.StickedToGameObject = StickedToGameObject;
            this.LastFrameStickedHandGameObject = LastFrameStickedHandGameObject;
            this.LastFrameStickedHandTransform = (LastFrameStickedHandGameObject == null) ? null : LastFrameStickedHandGameObject.transform;
        }
    }
    
    [Header("Functionality references")]
    [Tooltip("Attached pickupAble component, leave empty if there is none")]
    [SerializeField] private PickupAble _attachedPickupAble;
    [Tooltip("Attached HandStickAble components, leave empty if there is none")]
    [SerializeField] private HandStickAble[] _attachedHandStickAbles;
    
    private bool _isPickupAble;
    private HandFunctionality _lastFrameHeldByHand;
    
    private bool _isHandStickAble;
    private HandFunctionality[] _lastFrameStickedHands;
    private Hand[] _currentlyStickedHands;  //if hand with gameobject: exists, if hand withoput gameobject: nonextistent, if null: deleted before send

    private void Awake()
    {
        //set isPickupAble state
        _isPickupAble = _attachedPickupAble != null;
        //set isHandStickAble state
        _isHandStickAble = Array.FindIndex(_attachedHandStickAbles, h => h != null) != -1;

        _lastFrameStickedHands = new HandFunctionality[_attachedHandStickAbles.Length];
        _currentlyStickedHands = new Hand[_attachedHandStickAbles.Length];
    }

    private void Update()
    {
        if (_isPickupAble)  //PickupAbleMessages
        {
            //Send message containing Hand holding the pickupable, if none null (only if change has happened
            if (_lastFrameHeldByHand != _attachedPickupAble.currentHeldByHand)
            {
                GameObject currentHeldByGameObject = (_attachedPickupAble.currentHeldByHand == null) ? null : _attachedPickupAble.currentHeldByHand.gameObject;
                Hand currentHeldByHand = new Hand(currentHeldByGameObject);
                gameObject.SendMessage("OnHeldByHandChanged", currentHeldByHand, SendMessageOptions.DontRequireReceiver);
            }
            _lastFrameHeldByHand = _attachedPickupAble.currentHeldByHand;
        }
        
        if (_isHandStickAble) //HandSnapAbleMessages
        {
            //Send message containing hand array if any of the stick points have been changed
            for (int i = 0; i < _attachedHandStickAbles.Length; ++i)
            {
                if (_attachedHandStickAbles[i] == null)
                    continue;
                HandFunctionality lastFrameStickedHand = _lastFrameStickedHands[i];
                HandFunctionality stickedHand = _attachedHandStickAbles[i].AttachedFromHandFunctionality;
                if (lastFrameStickedHand != stickedHand)
                {
                    if (stickedHand == null)
                    {
                        _currentlyStickedHands[i] = new Hand(LastFrameStickedHandGameObject: lastFrameStickedHand.gameObject);
                    }
                    else
                    {
                        GameObject stickedGameObject = (stickedHand == null) ? null : stickedHand.gameObject;
                        GameObject stickedToGameObject = _attachedHandStickAbles[i].gameObject;
                        GameObject lastFrameStickedHandGameObject = (lastFrameStickedHand == null) ? null : lastFrameStickedHand.gameObject;
                        _currentlyStickedHands[i] = new Hand(stickedGameObject, stickedToGameObject, lastFrameStickedHandGameObject);
                    }
                    Hand[] nullValuesRemoved = Array.FindAll(_currentlyStickedHands, h => h != null);
                    gameObject.SendMessage("OnStickedHandsChanged", nullValuesRemoved, SendMessageOptions.DontRequireReceiver);
                }
                _lastFrameStickedHands[i] = stickedHand;
            }
        }
    }
}
