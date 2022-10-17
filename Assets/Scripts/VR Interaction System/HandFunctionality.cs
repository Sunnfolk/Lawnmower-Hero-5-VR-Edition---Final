using System;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(SteamVR_Behaviour_Pose))]
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(FixedJoint))]
[RequireComponent(typeof(HandController))]
public class HandFunctionality : MonoBehaviour
{
    /*
    Makes the controller be able to read input from HandController script, and supports following features:
    * Pick up objects with the pickUpAble script (and they will break off if you try to move them through walls)
    */
    
    [Header("Hand Options")] 
    [Tooltip("Radius of the sphere detecting pickupAbles (GrabRangeAmount)")]
    [SerializeField] private float _grabRadius = 0.1f;

    [Header("Object References")] 
    [Tooltip("The visual hand of this controller (must be a child of this GameObject, and have a HandVisual component")]
    [SerializeField] private HandVisual _handVisual;
    [Tooltip("The pauseMenu script enabling this controller to pause the game")]
    [SerializeField] private pauseEffect _pauseMenu;

    public PickupAble CurrentlyHeldPickupAble { private get; set; }
    public HandStickAble CurrentlyAttachedToHandStickAble { private get; set; }

    public FixedJoint FixedJoint { get; private set; }
    public SteamVR_Behaviour_Pose BehaviourPose { get; private set; }
    private HandController _handController;

    private void Awake()
    {
        FixedJoint = GetComponent<FixedJoint>();
        _handController = GetComponent<HandController>();
        BehaviourPose = GetComponent<SteamVR_Behaviour_Pose>();
    }

    //Anders edit: Change back to OnTriggerButtonChanged if this stops working
    private void OnGripButtonChanged(bool isTriggerDown) //Message from HandController
    {
        if (isTriggerDown)
        {
            //Pick up pickupAble if not null
            PickupAble closestPickupAble = _handController.ClosestPickupAble;
            if (closestPickupAble != null)
            {
                closestPickupAble.AttachPickupAbleToController(this);
                _handVisual.SetClosedHandMesh(closestPickupAble.CustomClosedHandMesh);  //set grab mesh to custom
                return;
            }

            //Stick to Hand StickAble if not null, and if not already other controller sticked
            HandStickAble closestHandStickAble = _handController.ClosestHandStickAble;
            if (closestHandStickAble != null && closestHandStickAble.AttachedFromHandFunctionality == null)
            {
                closestHandStickAble.AttachControllerToHandStickAble(this, _handVisual);
                _handVisual.SetClosedHandMesh(closestHandStickAble.CustomClosedHandMesh);   //set grab mesh to custom
                return;
            }

            _handVisual.SetClosedHandMesh(null);    //set grab mesh to default
        }
        else
        {
            //Drop held pickupAble if not null
            if (CurrentlyHeldPickupAble != null)// && CurrentlyAttachedToHandStickAble == null)
            {
                CurrentlyHeldPickupAble.DetachPickupAbleFromController();
            }

            //Detach from Hand StickAble if not null
            if (CurrentlyAttachedToHandStickAble != null)
            {
                CurrentlyAttachedToHandStickAble.DetachControllerFromHandStickAble();
            }

            _handVisual.SetOpenHandMesh(null);  //set open hand mesh to default
        }
    }

    private void OnMenuButtonChanged(bool menuButtonState)
    {
        if (!menuButtonState || _pauseMenu == null)
            return;
        
        if (pauseEffect.GameIsPaused)
        {
            _pauseMenu.Resume();
        }
        else
        {
            _pauseMenu.Pause();
        }
    }

    private void OnJointBreak(float breakForce)
    {
        //Create new fixedjoint if the last one breaks
        FixedJoint = gameObject.AddComponent<FixedJoint>();
        if (CurrentlyHeldPickupAble == null)
            return;
        CurrentlyHeldPickupAble.DetachPickupAbleFromController();
    }

    private void OnValidate()
    {
        //Set sphere collider trigger radus to specified value in this script
        GetComponent<SphereCollider>().radius = _grabRadius;
        //The Rigidbody on the controllers must be kinematic to prevent whacky physics when objects are picked up
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb.isKinematic == false)
        {
            rb.isKinematic = true;
            Debug.Log($"Rigidbody in {gameObject.name} must be kinematic to prevent whacky physics. Automatically set to true in script");
        }
        //Check if handVisual gameobject is a child of this gameobject
        if (_handVisual != null && !_handVisual.transform.IsChildOf(transform))
        {
            Debug.LogError($"{_handVisual.name} must be a child of {gameObject.name} for the hand following funtionality to work");
        }
    }
}
