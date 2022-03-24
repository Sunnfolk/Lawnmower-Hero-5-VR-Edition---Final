using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HandStickAble : MonoBehaviour
{
    /*
    When controller is within the sphere collider of this object it will snap to the hand stick point
    When the controller is released it will detach again
    This class does not affect the vr movement itself or the steamvr_behaviour_pose
        (Instead it just childs the handVisual transform while it is sticked)
    Also if the actual vr position is too far away from the handVisual it will break away, just like when trigger released
    */

    [Header("Hand Stick Point Options")]
    [Tooltip("Transform the hand gets sticked to (defaults to this objects transform if null)")]
    [SerializeField] private Transform _stickPoint;
    [Tooltip("Radius of sphere collider detecting stick")] 
    [SerializeField] private float _stickRadius = 0.5f;
    [Tooltip("If actual controller pos gets further from stick point than this number it will get detached automatically")]
    [SerializeField] private float _detachDistance = 0.25f;
    
    [Header("Mesh References")]
    [Tooltip("Custom mesh for hand closed for this hand stickAble")]
    [SerializeField] private Mesh _customClosedHandMesh;
    
    [Header("Stick Point Visualization")]
    [Tooltip("Weither to show visualization of the stick point or not")]
    [SerializeField] private bool e_visualizeStickPoint;
    [Tooltip("The mesh being shown when visualizing the grab")]
    [SerializeField] private Mesh e_handMesh;

    public HandFunctionality AttachedFromHandFunctionality { get; private set; }
    private Transform _attachedHandTransform;
    private HandVisual _attachedHandVisual;

    public Mesh CustomClosedHandMesh => _customClosedHandMesh;

    private void Awake()
    {
        //default stick point if null
        _stickPoint = (_stickPoint == null) ? transform : _stickPoint;
    }

    private void Update()
    {
        //check if hand detach distace if hand connected
        if (_attachedHandTransform == null)
            return;
        if (!(Vector3.Distance(AttachedFromHandFunctionality.transform.position, _attachedHandTransform.transform.position) > _detachDistance))
            return;
        DetachControllerFromHandStickAble();
    }

    private void OnValidate()
    {
        //Send error if HandStickAble is on the same gameObject as a pickupable
        if (TryGetComponent(out PickupAble pickupAble))
        {
            Debug.LogError($"Script {this.GetType().Name} and {pickupAble.GetType().Name} cannot be on the same gameobject. " +
                           $"On gameobject called {gameObject.name}");
        }
        //Send error if more than one Handstickable is on the same gameobject
        if (GetComponents<HandStickAble>().Length > 1)
        {
            Debug.LogError($"There can only be one {GetType().Name} on the same gameObject. " +
                           $"Remove duplicates from {name} and set them as seperate children instead");
        }
        //Set sphere collider to trigger, and set radius
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = _stickRadius;
        if (sphereCollider.isTrigger == false)
        {
            sphereCollider.isTrigger = true;
            Debug.Log($"SphereCollider in {name} must be trigger. Automatically set to true in script {GetType().Name}.");
        }
    }

    private void OnDrawGizmos() //visualize stick point if enabled
    {
        if (!e_visualizeStickPoint)
            return;
        Mesh visualizationMesh = (e_handMesh == null) ? _customClosedHandMesh : e_handMesh;
        Transform visualizationTransform = (_stickPoint == null) ? transform : _stickPoint;
        Gizmos.DrawMesh(visualizationMesh, visualizationTransform.transform.position, visualizationTransform.transform.rotation);
    }

    public void AttachControllerToHandStickAble(HandFunctionality handFunctionality, HandVisual handVisual)
    {
        //Exit if already held
        if (_attachedHandTransform != null)
            return;
        
        //stick to closest
        handVisual.transform.position = _stickPoint.position;
        handVisual.transform.rotation = _stickPoint.rotation;
        handVisual.transform.parent = _stickPoint;
        
        //Set sticked values
        handFunctionality.CurrentlyAttachedToHandStickAble = this;
        AttachedFromHandFunctionality = handFunctionality;
        _attachedHandTransform = handVisual.transform;
        _attachedHandVisual = _attachedHandTransform.TryGetComponent(out HandVisual hv) ? hv : null;
    }

    public void DetachControllerFromHandStickAble()
    {
        //Detach
        _attachedHandTransform.position = AttachedFromHandFunctionality.transform.position;
        _attachedHandTransform.rotation = AttachedFromHandFunctionality.transform.rotation;
        _attachedHandTransform.parent = AttachedFromHandFunctionality.transform;
        
        //Reset scale of visualHand to avoid huge hands
        if (_attachedHandVisual != null)
        {
            _attachedHandVisual.ResetScaleToAwakeScale();
        }
        
        //Reset sticked values
        AttachedFromHandFunctionality.CurrentlyAttachedToHandStickAble = null;
        AttachedFromHandFunctionality = null;
        _attachedHandTransform = null;
    }
}
