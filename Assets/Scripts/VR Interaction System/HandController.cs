using System;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
[RequireComponent(typeof(SphereCollider))]
public class HandController : MonoBehaviour
{
    /*
    This simplify getting action values by reducing them to simple get properties
    Only the input on the controller this script is attached are handled, so one script for each controller is required
    The reason I made the actions into properties is because I find it cleaner and more efficient than putting them in update each frame 
    */
    [Header("Button References")]
    [SerializeField] private SteamVR_Action_Boolean _triggerButtonAction;
    [SerializeField] private SteamVR_Action_Boolean _trackpadButtonAction;
    [SerializeField] private SteamVR_Action_Boolean _menuButtonAction;
    [SerializeField] private SteamVR_Action_Boolean _gripButtonAction;
    
    private List<PickupAble> _pickupAblesInControllerRadius = new List<PickupAble>();
    private List<InteractAble> _interactAblesInControllerRadius = new List<InteractAble>();
    private List<HandStickAble> _handStickAblesInControllerRadius = new List<HandStickAble>();

    private GameObject[] _interactAbleGameObjectsInHierarchy;

    public PickupAble ClosestPickupAble => CheckGetClosestObject(_pickupAblesInControllerRadius);   //might be null
    public InteractAble ClosestInteractAble => CheckGetClosestObject(_interactAblesInControllerRadius); //might be null
    public HandStickAble ClosestHandStickAble => CheckGetClosestObject(_handStickAblesInControllerRadius);   //might be null

    private SteamVR_Behaviour_Pose _behaviourPose;

    private void Awake()
    {
        _behaviourPose = GetComponent<SteamVR_Behaviour_Pose>();

        _interactAbleGameObjectsInHierarchy = Array.ConvertAll(FindObjectsOfType<InteractAble>(), i => i.gameObject);
    }

    private void OnTriggerEnter(Collider other) //Update Trigger content lists
    {
        CheckAddTypeToList(ref _pickupAblesInControllerRadius, other);
        CheckAddTypeToList(ref _interactAblesInControllerRadius, other);
        CheckAddTypeToList(ref _handStickAblesInControllerRadius, other);
    }
    private void CheckAddTypeToList<T>(ref List<T> listToAddTo, Collider colliderToCheck) where T : MonoBehaviour
    {
        if (colliderToCheck.TryGetComponent(out T typeInstance))
        {
            listToAddTo.Add(typeInstance);
        }
    }

    private void OnTriggerExit(Collider other)  //Update Trigger content lists
    {
        CheckRemoveTypeFromList(ref _pickupAblesInControllerRadius, other);
        CheckRemoveTypeFromList(ref _interactAblesInControllerRadius, other);
        CheckRemoveTypeFromList(ref _handStickAblesInControllerRadius, other);
    }
    private void CheckRemoveTypeFromList<T>(ref List<T> listToRemoveFrom, Collider colliderToCheck) where T : MonoBehaviour
    {
        if (colliderToCheck.TryGetComponent(out T typeInstance))
        {
            listToRemoveFrom.Remove(typeInstance);
        }
    }

    private void OnEnable() //Add listeners for input changes when enabled
    {
        AddOnChangeListenerIfNotNull(_triggerButtonAction, SendMessageOnTriggerButtonChanged);
        AddOnChangeListenerIfNotNull(_trackpadButtonAction, SendMessageOnTrackpadButtonChanged);
        AddOnChangeListenerIfNotNull(_menuButtonAction, SendMessageOnMenuButtonChanged);
        AddOnChangeListenerIfNotNull(_gripButtonAction, SendMessageOnGripButtonChanged);
    }
    private void AddOnChangeListenerIfNotNull(SteamVR_Action_Boolean actionBoolean, SteamVR_Action_Boolean.ChangeHandler functionToCall)
    {
        if (actionBoolean == null)
            return;
        actionBoolean.AddOnChangeListener(functionToCall, _behaviourPose.inputSource);
    }

    private void OnDisable()    //remove listeners if disabled
    {
        RemoveOnChangeListenerIfNotNull(_triggerButtonAction, SendMessageOnTriggerButtonChanged);
        RemoveOnChangeListenerIfNotNull(_trackpadButtonAction, SendMessageOnTrackpadButtonChanged);
        RemoveOnChangeListenerIfNotNull(_menuButtonAction, SendMessageOnMenuButtonChanged);
        RemoveOnChangeListenerIfNotNull(_gripButtonAction, SendMessageOnGripButtonChanged);
    }
    private void RemoveOnChangeListenerIfNotNull(SteamVR_Action_Boolean actionBoolean, SteamVR_Action_Boolean.ChangeHandler functionToCall)
    {
        if (actionBoolean == null)
            return;
        actionBoolean.RemoveOnChangeListener(functionToCall, _behaviourPose.inputSource);
    }

    private void OnValidate()
    {
        //The spherecollider must be a trigger to prevent actual collisions
        SphereCollider sc = GetComponent<SphereCollider>();
        if (!sc.isTrigger)
        {
            sc.isTrigger = true;
            Debug.Log($"SphereCollider in {gameObject.name} must be a trigger. Automatically set to true by script");
        }
    }

    private T CheckGetClosestObject<T>(List<T> objectList) where T : MonoBehaviour  //Null check and get closest object of type, null if none present
    {
        if (objectList.Count == 0)
        {
            return null;
        }

        //find closest pickupAble from pickupAble withing trigger list
        float closestDistance = float.MaxValue;
        T closestObject = null;
        foreach (T objectOfType in objectList)
        {
            if (objectOfType == null)
            {
                continue;
            }

            float distance = Vector3.Distance(objectOfType.transform.position, transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestObject = objectOfType;
            }
        }

        return closestObject;
    }

    private void SendMessageOnButtonChanged(string functionName, bool state)    //Sends message about change to this gameobject, and the interactable in hierarchy
    {
        SendMessage(functionName, state, SendMessageOptions.DontRequireReceiver);   //send message to this gameobject

        foreach (GameObject interactAbleGameObject in _interactAbleGameObjectsInHierarchy)  //Send message to every interactable object in hierarchy (only the ones added on awake)
        {
            interactAbleGameObject.SendMessage(functionName, state, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void SendMessageOnTriggerButtonChanged(SteamVR_Action_Boolean actionBoolean, SteamVR_Input_Sources inputSources, bool state)
    {
        SendMessageOnButtonChanged("OnTriggerButtonChanged", state);
    }

    private void SendMessageOnTrackpadButtonChanged(SteamVR_Action_Boolean actionBoolean, SteamVR_Input_Sources inputSources, bool state)
    {
        SendMessageOnButtonChanged("OnTrackpadButtonChanged", state);
    }

    private void SendMessageOnMenuButtonChanged(SteamVR_Action_Boolean actionBoolean, SteamVR_Input_Sources inputSources, bool state)
    {
        SendMessageOnButtonChanged("OnMenuButtonChanged", state);
    }

    private void SendMessageOnGripButtonChanged(SteamVR_Action_Boolean actionBoolean, SteamVR_Input_Sources inputSources, bool state)
    {
        SendMessageOnButtonChanged("OnGripButtonChanged", state);
    }
}
