using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PickupAble : MonoBehaviour
{
    /*
    Any object with this script can be picked up by the controller
    And will detach if enough force is applied (like when you try to move it through a wall)
    */
    [Header("Options")] [Tooltip("Child of gameobject showing where the hold point is relative to controller (null if no specification needed")] 
    [SerializeField] private Transform _holdPoint;
    [Tooltip("Force required to detach object on collisions (can be set to infinity to make unbreakable) (is applied directly to the fixed joint of controller)")]
    [SerializeField] private float _breakForceToDetach = 2500f;

    [Header("Hold Mesh")]
    [Tooltip("Custom controller hold mesh for this PickupAble")]
    [SerializeField] private Mesh _customlosedHandMesh;

    [Header("Hold Point Visualization")]
    [Tooltip("Weither to visualize mesh hold point or not")] 
    [SerializeField] private bool e_visualizeHoldPoint;
    [Tooltip("Mash of the controller holding the object (defaults to closed hand mesh if not set")]
    [SerializeField] private Mesh e_controllerMesh;
    
    public Mesh CustomClosedHandMesh => _customlosedHandMesh;

    [NonSerialized] public HandFunctionality currentHeldByHand;
    [NonSerialized] public bool heldSinceRespawn;
    [NonSerialized] public float lastHeldFixedTime;

    private Vector3 _holdPointPosModifier;
    private Vector3 _holdPointRotModifier;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        //Nullsafe hold point
        _holdPoint = (_holdPoint == null) ? transform : _holdPoint;

        _holdPointPosModifier = _holdPoint.position - transform.position;
        _holdPointRotModifier = _holdPoint.eulerAngles - transform.eulerAngles;
    }

    private void Start()
    {
        SetPickedUpYetState(false);
    }

    private void OnValidate()
    {
        //Send error if hold point is not a child of this gameobject
        if (_holdPoint != null && !_holdPoint.IsChildOf(transform))
        {
            Debug.LogError($"hold point {_holdPoint.name} must be a child of {gameObject.name} " +
                           $"for the hold point functionality to work");
        }
    }

    private void OnDrawGizmos() //visualize hold point
    {
        if (!e_visualizeHoldPoint)
            return;
        Mesh holdMesh = (e_controllerMesh == null) ? CustomClosedHandMesh : e_controllerMesh;
        Transform holdPoint = (_holdPoint == null) ? transform : _holdPoint;
        Vector3 pos = transform.position + (holdPoint.position - transform.position);
        Quaternion rot = transform.rotation * holdPoint.rotation * Quaternion.Inverse(transform.rotation);
        Gizmos.DrawMesh(holdMesh, pos, rot);
    }

    public void AttachPickupAbleToController(HandFunctionality handFunctionality)
    {
        //Detach pickupable from already held hand if it is already held
        if (currentHeldByHand != null)
        {
            DetachPickupAbleFromController();
        }
        
        //Set held values
        currentHeldByHand = handFunctionality;
        currentHeldByHand.CurrentlyHeldPickupAble = this;
        
        //enable picked up yet mode
        SetPickedUpYetState(true);

        //Attach PickupAble to controller fixed joint, and disable velocity to avoid the fixed joint breaking
        _rigidBody.velocity = Vector3.zero;
        _rigidBody.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(handFunctionality.transform.position, handFunctionality.transform.rotation);
        transform.Translate(-_holdPointPosModifier);
        transform.Rotate(-_holdPointRotModifier);
        
        currentHeldByHand.FixedJoint.connectedBody = _rigidBody;
        currentHeldByHand.FixedJoint.breakForce = _breakForceToDetach;
    }
    
    public void DetachPickupAbleFromController()
    {
        //Detach from controller fixed joint
        currentHeldByHand.FixedJoint.connectedBody = null;
        
        //Set pickupAble held since respawn to true
        heldSinceRespawn = true;

        //reset pickupAble seconds since last held
        lastHeldFixedTime = Time.fixedTime;
        
        //Apply velocity of controllers
        _rigidBody.velocity = currentHeldByHand.BehaviourPose.GetVelocity();
        _rigidBody.angularVelocity = currentHeldByHand.BehaviourPose.GetAngularVelocity();
        
        //Clear held values
        currentHeldByHand.CurrentlyHeldPickupAble = null;
        currentHeldByHand = null;
    }

    public void SetPickedUpYetState(bool state)
    {
        _rigidBody.isKinematic = !state;
    }
}
